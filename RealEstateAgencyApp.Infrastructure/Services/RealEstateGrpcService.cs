using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealEstateAgencyApp.Contracts.Dtos.RequestDtos;
using RealEstateAgencyApp.Contracts.Grpc;
using RealEstateAgencyApp.Contracts.Interfaces;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Entities.Enums;
using RealEstateAgencyApp.Domain.Interfaces;

namespace RealEstateAgencyApp.Infrastructure.Services;

/// <summary>
/// gRPC service for handling real estate streaming requests
/// </summary>
public class RealEstateGrpcService : RealEstateStreaming.RealEstateStreamingBase
{
    private readonly ILogger<RealEstateGrpcService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RealEstateGrpcService(
        ILogger<RealEstateGrpcService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task StreamRequests(
        IAsyncStreamReader<RequestStreamMessage> requestStream,
        IServerStreamWriter<ResponseStreamMessage> responseStream,
        ServerCallContext context)
    {
        _logger.LogInformation("gRPC streaming session started");

        var processedCount = 0;
        var errors = new List<string>();

        await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
        {
            try
            {
                _logger.LogInformation(
                    "Processing request: counterparty {CounterpartyId}, estate {EstateId}, type {Type}",
                    request.CounterpartyId, request.EstateId, request.Type);

                var success = await ProcessRequestAsync(request);

                if (success)
                {
                    processedCount++;
                    await responseStream.WriteAsync(new ResponseStreamMessage
                    {
                        Success = true,
                        Message = $"Successfully processed request #{processedCount}",
                        Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
                    });

                    _logger.LogInformation("Successfully processed request #{ProcessedCount}", processedCount);
                }
                else
                {
                    var errorMsg = $"Failed to process request: counterparty or estate not found";
                    errors.Add(errorMsg);

                    await responseStream.WriteAsync(new ResponseStreamMessage
                    {
                        Success = false,
                        Message = errorMsg,
                        Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
                    });

                    _logger.LogWarning(errorMsg);
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error processing request: {ex.Message}";
                errors.Add(errorMsg);

                await responseStream.WriteAsync(new ResponseStreamMessage
                {
                    Success = false,
                    Message = errorMsg,
                    Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
                });

                _logger.LogError(ex, "Error processing gRPC request");
            }
        }

        _logger.LogInformation(
            "gRPC streaming session completed. Processed: {ProcessedCount}, Errors: {ErrorCount}",
            processedCount, errors.Count);
    }

    private async Task<bool> ProcessRequestAsync(RequestStreamMessage request)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();
        var counterpartyRepository = scope.ServiceProvider.GetRequiredService<ICounterpartyRepository>();
        var estateRepository = scope.ServiceProvider.GetRequiredService<IRealEstateObjectRepository>();

        try
        {
            var counterparty = await counterpartyRepository.GetByIdAsync(request.CounterpartyId);
            var estate = await estateRepository.GetByIdAsync(request.EstateId);

            if (counterparty == null || estate == null)
            {
                _logger.LogWarning(
                    "Counterparty {CounterpartyId} or Estate {EstateId} not found",
                    request.CounterpartyId, request.EstateId);
                return false;
            }

            var createDto = new RequestEditDto
            {
                Counterparty = counterparty,
                Estate = estate,
                Type = Enum.Parse<RequestType>(request.Type),
                Price = (decimal)request.Price,
                Date = request.Date.ToDateTime()
            };

            var result = await requestService.CreateAsync(createDto);

            _logger.LogInformation(
                "Created request {RequestId} for counterparty {CounterpartyName}, estate {CadastralNumber}",
                result.Id, counterparty.FullName, estate.CadastralNumber);

            return true;
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning(
                "Counterparty {CounterpartyId} or Estate {EstateId} not found",
                request.CounterpartyId, request.EstateId);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error creating request for counterparty {CounterpartyId}, estate {EstateId}",
                request.CounterpartyId, request.EstateId);
            return false;
        }
    }
}
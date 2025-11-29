using Grpc.Core;
using RealEstateAgencyApp.Contracts.Grpc;
using RealEstateAgencyApp.Contracts.Interfaces;
using RealEstateAgencyApp.Contracts.Dtos.RequestDtos;
using RealEstateAgencyApp.Domain.Entities.Enums;
using RealEstateAgencyApp.Domain.Interfaces;

namespace RealEstateAgencyApp.GrpcConsumer.Services;

public class RealEstateStreamingService : RealEstateStreaming.RealEstateStreamingBase
{
    private readonly ILogger<RealEstateStreamingService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public RealEstateStreamingService(
        ILogger<RealEstateStreamingService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public override async Task StreamRequests(
        IAsyncStreamReader<RequestStreamMessage> requestStream,
        IServerStreamWriter<ResponseStreamMessage> responseStream,
        ServerCallContext context)
    {
        _logger.LogInformation("Started bidirectional streaming from {Peer}", context.Peer);

        try
        {
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                _logger.LogInformation("Received: counterparty {CounterpartyId}, estate {EstateId}, type {Type}, price {Price}",
                    request.CounterpartyId, request.EstateId, request.Type, request.Price);

                var success = await SaveRequestViaService(request);

                await responseStream.WriteAsync(new ResponseStreamMessage
                {
                    Success = success,
                    Message = success ?
                        $"Request saved for counterparty {request.CounterpartyId}" :
                        $"Failed to save request for counterparty {request.CounterpartyId}",
                    Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
                });

                _logger.LogInformation("Sent response for counterparty {CounterpartyId}", request.CounterpartyId);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Streaming cancelled by client {Peer}", context.Peer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during streaming");
            throw;
        }

        _logger.LogInformation("Finished streaming requests");
    }

    private async Task<bool> SaveRequestViaService(RequestStreamMessage request)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();

            var requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();
            var counterpartyRepository = scope.ServiceProvider.GetRequiredService<ICounterpartyRepository>();
            var estateRepository = scope.ServiceProvider.GetRequiredService<IRealEstateObjectRepository>();

            var counterparty = await counterpartyRepository.GetByIdAsync(request.CounterpartyId);
            var estate = await estateRepository.GetByIdAsync(request.EstateId);

            if (counterparty == null)
            {
                _logger.LogWarning("Counterparty {CounterpartyId} not found", request.CounterpartyId);
                return false;
            }
            if (estate == null)
            {
                _logger.LogWarning("Estate {EstateId} not found", request.EstateId);
                return false;
            }

            if (!Enum.TryParse<RequestType>(request.Type, out var requestType))
            {
                _logger.LogWarning("Invalid request type: {Type}", request.Type);
                return false;
            }

            if (request.Price <= 0)
            {
                _logger.LogWarning("Invalid price: {Price}", request.Price);
                return false;
            }

            var requestDto = new RequestEditDto
            {
                Counterparty = counterparty,
                Estate = estate,
                Type = requestType,
                Price = (decimal)request.Price,
                Date = request.Date.ToDateTime()
            };

            await requestService.CreateAsync(requestDto);

            _logger.LogInformation("Successfully saved request via service for counterparty {CounterpartyId}, estate {EstateId}",
                request.CounterpartyId, request.EstateId);
            return true;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Counterparty or Estate not found for request");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save request via service for counterparty {CounterpartyId}",
                request.CounterpartyId);
            return false;
        }
    }
}
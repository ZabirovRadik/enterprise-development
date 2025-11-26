using Grpc.Core;
using RealEstateAgencyApp.Contracts.Grpc;

namespace RealEstateAgencyApp.GrpcConsumer.Services;

public class RealEstateStreamingService : RealEstateStreaming.RealEstateStreamingBase
{
    private readonly ILogger<RealEstateStreamingService> _logger;

    public RealEstateStreamingService(ILogger<RealEstateStreamingService> logger)
    {
        _logger = logger;
    }

    public override async Task StreamRequests(
        IAsyncStreamReader<RequestStreamMessage> requestStream,
        IServerStreamWriter<ResponseStreamMessage> responseStream,
        ServerCallContext context)
    {
        _logger.LogInformation("Started bidirectional streaming requests from {Peer}", context.Peer);

        try
        {
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                _logger.LogInformation("Received request: counterparty {CounterpartyId}, estate {EstateId}, type {Type}, price {Price}",
                    request.CounterpartyId, request.EstateId, request.Type, request.Price);

                var success = true;

                await responseStream.WriteAsync(new ResponseStreamMessage
                {
                    Success = success,
                    Message = $"Request processed for counterparty {request.CounterpartyId}",
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
            _logger.LogError(ex, "Error during streaming requests");
            throw;
        }

        _logger.LogInformation("Finished streaming requests");
    }
}
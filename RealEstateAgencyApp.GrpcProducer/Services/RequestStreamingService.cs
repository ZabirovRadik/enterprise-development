using Bogus;
using Grpc.Core;
using Microsoft.Extensions.Options;
using RealEstateAgencyApp.Contracts.Grpc;
using RealEstateAgencyApp.GrpcProducer.Configurations;

namespace RealEstateAgencyApp.GrpcProducer.Services;

/// <summary>
/// Service for generating real estate requests and sending them via gRPC.
/// </summary>
public class RequestGeneratorService(
    ILogger<RequestGeneratorService> logger,
    RealEstateStreaming.RealEstateStreamingClient client,
    IOptions<GeneratorOptions> options)
{
    private readonly GeneratorOptions _options = options.Value;

    /// <summary>
    /// Starts automatic generation of real estate requests.
    /// Uses settings from configuration for batch size and timing.
    /// </summary>
    public async Task GenerateAutomatically(CancellationToken stoppingToken = default)
    {
        logger.LogInformation("Starting automatic generation: batchSize={BatchSize}, limit={Limit}, wait={Wait}s",
            _options.BatchSize, _options.PayloadLimit, _options.WaitTime);

        var counter = 0;

        while (counter < _options.PayloadLimit && !stoppingToken.IsCancellationRequested)
        {
            var success = await GenerateAndSendRequests(_options.BatchSize, stoppingToken);
            if (success)
            {
                counter += _options.BatchSize;
                logger.LogDebug("Sent batch of {BatchSize} requests. Total: {Total}", _options.BatchSize, counter);
            }

            await Task.Delay(_options.WaitTime * 100, stoppingToken);
        }

        logger.LogInformation("Automatic generation finished. Total sent: {Total}", counter);
    }

    /// <summary>
    /// Generates and sends a batch of requests via gRPC streaming.
    /// Retries up to 3 times if sending fails.
    /// </summary>
    private async Task<bool> GenerateAndSendRequests(int count, CancellationToken stoppingToken = default)
    {
        var retryCount = 0;

        logger.LogDebug("Starting generation of {Count} requests", count);

        while (retryCount < _options.MaxRetries && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                var faker = new Faker();
                using var call = client.StreamRequests(
                    deadline: DateTime.UtcNow.AddSeconds(_options.GrpcTimeoutSeconds),
                    cancellationToken: stoppingToken);

                for (var i = 0; i < count; i++)
                {
                    var request = new RequestStreamMessage
                    {
                        CounterpartyId = faker.Random.Int(
                            _options.Data.CounterpartyIdRange.Min,
                            _options.Data.CounterpartyIdRange.Max),
                        EstateId = faker.Random.Int(
                            _options.Data.EstateIdRange.Min,
                            _options.Data.EstateIdRange.Max),
                        Type = faker.PickRandom(_options.Data.RequestTypes),
                        Price = faker.Random.Int(
                            _options.Data.PriceRange.Min,
                            _options.Data.PriceRange.Max),
                        Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
                    };

                    await call.RequestStream.WriteAsync(request, stoppingToken);
                    logger.LogDebug("Sent request {Count} with counterparty {CounterpartyId}, estate {EstateId}",
                        i + 1, request.CounterpartyId, request.EstateId);
                }

                await call.RequestStream.CompleteAsync();

                var responses = new List<ResponseStreamMessage>();
                await foreach (var response in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    responses.Add(response);
                    logger.LogDebug("Received response: {Success} - {Message}", response.Success, response.Message);
                }

                logger.LogInformation("Successfully completed batch with {ResponseCount} responses", responses.Count);
                return true;
            }
            catch (Exception ex)
            {
                retryCount++;
                logger.LogWarning(ex, "Failed to send batch (attempt {RetryCount}/{MaxRetries})",
                    retryCount, _options.MaxRetries);

                if (retryCount < _options.MaxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(_options.RetryDelaySeconds), stoppingToken);
                }
                else
                {
                    logger.LogError(ex, "Failed to send batch after {MaxRetries} attempts", _options.MaxRetries);
                    return false;
                }
            }
        }

        return false;
    }
}
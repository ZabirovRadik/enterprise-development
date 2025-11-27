using Bogus;
using Grpc.Core;
using RealEstateAgencyApp.Contracts.Grpc;

namespace RealEstateAgencyApp.GrpcProducer.Services;

/// <summary>
/// Service for generating real estate requests and sending them via gRPC.
/// </summary>
public class RequestGeneratorService
{
    private readonly ILogger<RequestGeneratorService> _logger;
    private readonly RealEstateStreaming.RealEstateStreamingClient _client;
    private readonly IConfiguration _configuration;

    public RequestGeneratorService(
        ILogger<RequestGeneratorService> logger,
        RealEstateStreaming.RealEstateStreamingClient client,
        IConfiguration configuration)
    {
        _logger = logger;
        _client = client;
        _configuration = configuration;
    }

    /// <summary>
    /// Starts automatic generation of real estate requests.
    /// Uses settings from configuration for batch size and timing.
    /// </summary>
    public async Task GenerateAutomatically(CancellationToken stoppingToken = default)
    {
        var batchSize = _configuration.GetValue<int>("Generator:BatchSize", 5);
        var payloadLimit = _configuration.GetValue<int>("Generator:PayloadLimit", 20);
        var waitTime = _configuration.GetValue<int>("Generator:WaitTime", 3);

        _logger.LogInformation("Starting automatic generation: batchSize={BatchSize}, limit={Limit}, wait={Wait}s",
            batchSize, payloadLimit, waitTime);

        var counter = 0;

        while (counter < payloadLimit && !stoppingToken.IsCancellationRequested)
        {
            var success = await GenerateAndSendRequests(batchSize, stoppingToken);
            if (success)
            {
                counter += batchSize;
                _logger.LogInformation("Sent batch of {BatchSize} requests. Total: {Total}", batchSize, counter);
            }

            await Task.Delay(waitTime * 1000, stoppingToken);
        }

        _logger.LogInformation("Automatic generation finished. Total sent: {Total}", counter);
    }

    /// <summary>
    /// Generates and sends a batch of requests via gRPC streaming.
    /// Retries up to 3 times if sending fails.
    /// </summary>
    private async Task<bool> GenerateAndSendRequests(int count, CancellationToken stoppingToken = default)
    {
        var maxRetries = _configuration.GetValue<int>("Generator:MaxRetries", 3);
        var retryDelaySeconds = _configuration.GetValue<int>("Generator:RetryDelaySeconds", 5);
        var retryCount = 0;

        _logger.LogInformation("Starting generation of {Count} requests", count);
        var dataConfig = _configuration.GetSection("Generator:Data");
        var grpcTimeout = _configuration.GetValue<int>("Generator:GrpcTimeoutSeconds", 30);
        while (retryCount < maxRetries && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                var faker = new Faker();
                using var call = _client.StreamRequests(deadline: DateTime.UtcNow.AddSeconds(grpcTimeout));

                for (var i = 0; i < count; i++)
                {
                    var request = new RequestStreamMessage
                    {
                        CounterpartyId = faker.Random.Int(
                            dataConfig.GetValue<int>("CounterpartyIdRange:Min", 1),
                            dataConfig.GetValue<int>("CounterpartyIdRange:Max", 10)
                        ),
                        EstateId = faker.Random.Int(
                            dataConfig.GetValue<int>("EstateIdRange:Min", 1),
                            dataConfig.GetValue<int>("EstateIdRange:Max", 10)
                        ),
                        Type = faker.PickRandom(
                            dataConfig.GetSection("RequestTypes").Get<string[]>() ?? new[] { "Buy", "Sell" }
                        ),
                        Price = faker.Random.Int(
                            dataConfig.GetValue<int>("PriceRange:Min", 100000),
                            dataConfig.GetValue<int>("PriceRange:Max", 5000000)
                        ),
                        Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
                    };

                    await call.RequestStream.WriteAsync(request);
                    _logger.LogInformation("Sent request {Count} with counterparty {CounterpartyId}, estate {EstateId}",
                        i + 1, request.CounterpartyId, request.EstateId);
                }

                await call.RequestStream.CompleteAsync();

                var responses = new List<ResponseStreamMessage>();
                await foreach (var response in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    responses.Add(response);
                    _logger.LogInformation("Received response: {Success} - {Message}", response.Success, response.Message);
                }

                _logger.LogInformation("Successfully completed batch with {ResponseCount} responses", responses.Count);
                return true;
            }
            catch (Exception ex)
            {
                retryCount++;
                _logger.LogWarning(ex, "Failed to send batch (attempt {RetryCount}/{MaxRetries})", retryCount, maxRetries);

                if (retryCount < maxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(retryDelaySeconds), stoppingToken);
                }
                else
                {
                    _logger.LogError(ex, "Failed to send batch after {MaxRetries} attempts", maxRetries);
                    return false;
                }
            }
        }

        return false;
    }
}
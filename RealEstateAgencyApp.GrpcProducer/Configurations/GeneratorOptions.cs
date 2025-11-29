namespace RealEstateAgencyApp.GrpcProducer.Configurations;

public class GeneratorOptions
{
    public int BatchSize { get; set; } = 5;
    public int PayloadLimit { get; set; } = 20;
    public int WaitTime { get; set; } = 3;
    public int MaxRetries { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 5;
    public int GrpcTimeoutSeconds { get; set; } = 30;
    public DataOptions Data { get; set; } = new();
}

public class DataOptions
{
    public RangeOptions CounterpartyIdRange { get; set; } = new(1, 10);
    public RangeOptions EstateIdRange { get; set; } = new(1, 10);
    public RangeOptions PriceRange { get; set; } = new(100000, 5000000);
    public string[] RequestTypes { get; set; } = ["Buy", "Sell"];
}

public record RangeOptions(int Min, int Max);
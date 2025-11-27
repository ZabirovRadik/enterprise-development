using Grpc.Net.Client;
using RealEstateAgencyApp.Contracts.Grpc;
using RealEstateAgencyApp.GrpcProducer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton(serviceProvider =>
{
    var grpcServiceUrl = builder.Configuration["Grpc:ServiceUrl"]
           ?? throw new InvalidOperationException("Grpc:ServiceUrl is not configured");
    var httpHandler = new HttpClientHandler();

    httpHandler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

    var channel = GrpcChannel.ForAddress(grpcServiceUrl, new GrpcChannelOptions
    {
        HttpHandler = httpHandler
    });

    return new RealEstateStreaming.RealEstateStreamingClient(channel);
});

builder.Services.AddSingleton<RequestGeneratorService>();

var app = builder.Build();
app.MapGet("/", () => Results.Redirect("/swagger"));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
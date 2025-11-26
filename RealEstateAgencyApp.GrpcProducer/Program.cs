using Grpc.Net.Client;
using RealEstateAgencyApp.Contracts.Grpc;
using RealEstateAgencyApp.GrpcProducer.Controllers;
using RealEstateAgencyApp.GrpcProducer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(serviceProvider =>
{
    var grpcServiceUrl = "https://localhost:7002";
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
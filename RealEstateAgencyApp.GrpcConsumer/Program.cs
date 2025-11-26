using RealEstateAgencyApp.GrpcConsumer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

var app = builder.Build();
app.MapGrpcService<RealEstateStreamingService>();
app.Run();
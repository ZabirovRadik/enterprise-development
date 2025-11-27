using Microsoft.EntityFrameworkCore;
using RealEstateAgencyApp.Application.Mappers;
using RealEstateAgencyApp.Application.Services;
using RealEstateAgencyApp.Contracts.Interfaces;
using RealEstateAgencyApp.Domain.Interfaces;
using RealEstateAgencyApp.GrpcConsumer.Services;
using RealEstateAgencyApp.Infrastructure.Persistence;
using RealEstateAgencyApp.Infrastructure.Repositories;
using RealEstateAgencyApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("mysqldb");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<ICounterpartyRepository, CounterpartyRepository>();
builder.Services.AddScoped<IRealEstateObjectRepository, RealEstateObjectRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddGrpc();

var app = builder.Build();
app.MapDefaultEndpoints();
app.MapGrpcService<RealEstateStreamingService>();
app.Run();
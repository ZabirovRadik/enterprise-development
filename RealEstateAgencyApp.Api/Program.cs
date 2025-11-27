using Microsoft.EntityFrameworkCore;
using RealEstateAgencyApp.Application.Mappers;
using RealEstateAgencyApp.Application.Services;
using RealEstateAgencyApp.Contracts.Dtos.CounterpartyDtos;
using RealEstateAgencyApp.Contracts.Dtos.RealEstateObjectDtos;
using RealEstateAgencyApp.Contracts.Interfaces;
using RealEstateAgencyApp.Domain.Interfaces;
using RealEstateAgencyApp.Infrastructure.Persistence;
using RealEstateAgencyApp.Infrastructure.Repositories;
using RealEstateAgencyApp.ServiceDefaults;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("mysqldb");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<ICounterpartyRepository, CounterpartyRepository>();
builder.Services.AddScoped<IRealEstateObjectRepository, RealEstateObjectRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();

builder.Services.AddScoped<ICrudService<CounterpartyGetDto, CounterpartyEditDto>, CounterpartyService>();
builder.Services.AddScoped<ICrudService<RealEstateObjectGetDto, RealEstateObjectEditDto>, RealEstateObjectService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();


builder.Services.AddAutoMapper(typeof(AppMappingProfile));

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();

    await DbSeeder.SeedAllAsync(context);
}

app.Run();
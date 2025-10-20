using Microsoft.EntityFrameworkCore;
using RealEstateAgencyApp.Application.Mappers;
using RealEstateAgencyApp.Application.Services;
using RealEstateAgencyApp.Domain.Interfaces;
using RealEstateAgencyApp.Infrastructure.Persistence;
using RealEstateAgencyApp.Infrastructure.Repositories;
using RealEstateAgencyApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<ICounterpartyRepository, CounterpartyRepository>();
builder.Services.AddScoped<IRealEstateObjectRepository, RealEstateObjectRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();

builder.Services.AddScoped<AnalyticsService>();

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
    var context = scope.ServiceProvider.GetRequiredService<DBContext>();

    context.Database.EnsureCreated();

    await DbSeeder.SeedAllAsync(context);
}

app.Run();
using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.GrpcProducer.Services;

namespace RealEstateAgencyApp.GrpcProducer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeneratorController : ControllerBase
{
    private readonly RequestGeneratorService _generatorService;
    private readonly ILogger<GeneratorController> _logger;

    public GeneratorController(
        RequestGeneratorService generatorService,
        ILogger<GeneratorController> logger)
    {
        _generatorService = generatorService;
        _logger = logger;
    }

    /// <summary>
    /// Запускает генерацию указанного количества контрактов
    /// </summary>
    [HttpPost("generate/{count:int}")]
    public async Task<ActionResult> GenerateContracts(int count)
    {
        try
        {
            _logger.LogInformation("Manual generation requested for {Count} contracts", count);

            var success = await _generatorService.GenerateAndSendRequests(count);

            return Ok(new
            {
                success = true,
                message = $"Generated and sent {count} contracts",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during manual generation");
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Запускает автоматическую генерацию по настройкам
    /// </summary>
    [HttpPost("auto")]
    public ActionResult StartAutoGeneration()
    {
        try
        {
            _logger.LogInformation("Auto generation started");

            // Запускаем в фоне
            _ = _generatorService.GenerateAutomatically();

            return Ok(new
            {
                success = true,
                message = "Auto generation started",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting auto generation");
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    [HttpGet("status")]
    public ActionResult GetStatus()
    {
        return Ok(new
        {
            status = "Generator service is ready",
            timestamp = DateTime.UtcNow
        });
    }
}
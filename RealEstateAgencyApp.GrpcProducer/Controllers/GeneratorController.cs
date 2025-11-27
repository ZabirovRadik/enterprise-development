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
    /// Starts automatic generation according to settings
    /// </summary>
    [HttpPost("auto")]
    public ActionResult StartAutoGeneration()
    {
        try
        {
            _logger.LogInformation("Auto generation started");

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
}
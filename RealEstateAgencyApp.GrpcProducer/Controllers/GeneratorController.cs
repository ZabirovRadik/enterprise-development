using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.GrpcProducer.Services;

namespace RealEstateAgencyApp.GrpcProducer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeneratorController(RequestGeneratorService generatorService,
        ILogger<GeneratorController> logger) : ControllerBase
{
    /// <summary>
    /// Starts automatic generation according to settings
    /// </summary>
    [HttpPost("auto")]
    public ActionResult StartAutoGeneration()
    {
        try
        {
            logger.LogInformation("Auto generation started");

            _ = generatorService.GenerateAutomatically();

            return Ok(new
            {
                success = true,
                message = "Auto generation started",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error starting auto generation");
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message
            });
        }
    }
}
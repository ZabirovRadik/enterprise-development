using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.Contracts.Dtos.AnalyticsDtos;
using RealEstateAgencyApp.Contracts.Interfaces;
using System.Net;

namespace RealEstateAgencyApp.API.Controllers;

/// <summary>
/// Analytics endpoints for real estate objects, counterparties, and requests.
/// </summary>
/// <param name="analyticsService">Service that provides analytics operations.</param>
/// <param name="logger">Logger for error logging.</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(IAnalyticsService analyticsService, ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// Returns the top counterparties by total request value.
    /// </summary>
    /// <param name="topCount">Number of top counterparties to return (default: 10).</param>
    [HttpGet("top-counterparties")]
    [ProducesResponseType(typeof(List<CounterpartyWithTotalValueDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<CounterpartyWithTotalValueDto>>> GetTopCounterparties([FromQuery] int topCount = 10)
    {
        try
        {
            if (topCount <= 0)
            {
                logger.LogWarning("GetTopCounterparties called with invalid topCount: {TopCount}", topCount);
                return BadRequest("topCount must be greater than 0");
            }

            var result = await analyticsService.GetTopCounterpartiesByValueAsync(topCount);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "ArgumentException in GetTopCounterparties. topCount: {TopCount}", topCount);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting top counterparties. topCount: {TopCount}", topCount);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Returns the most popular real estate objects by request count.
    /// </summary>
    /// <param name="topCount">Number of popular estates to return (default: 10).</param>
    [HttpGet("popular-estates")]
    [ProducesResponseType(typeof(List<RealEstateWithRequestCountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<RealEstateWithRequestCountDto>>> GetMostPopularEstates([FromQuery] int topCount = 10)
    {
        try
        {
            if (topCount <= 0)
            {
                logger.LogWarning("GetMostPopularEstates called with invalid topCount: {TopCount}", topCount);
                return BadRequest("topCount must be greater than 0");
            }

            var result = await analyticsService.GetMostPopularEstatesAsync(topCount);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "ArgumentException in GetMostPopularEstates. topCount: {TopCount}", topCount);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting most popular estates. topCount: {TopCount}", topCount);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Returns price statistics by real estate type.
    /// </summary>
    [HttpGet("price-statistics")]
    [ProducesResponseType(typeof(List<PriceStatsByTypeDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<PriceStatsByTypeDto>>> GetPriceStatistics()
    {
        try
        {
            var result = await analyticsService.GetPriceStatisticsByTypeAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting price statistics");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Returns monthly request statistics for the last year.
    /// </summary>
    [HttpGet("monthly-statistics")]
    [ProducesResponseType(typeof(List<MonthlyRequestsDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<MonthlyRequestsDto>>> GetMonthlyStatistics()
    {
        try
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddYears(-1);
            var result = await analyticsService.GetMonthlyStatisticsAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting monthly statistics");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Returns real estate objects with encumbrances that have active requests.
    /// </summary>
    [HttpGet("encumbered-estates")]
    [ProducesResponseType(typeof(List<RealEstateWithRequestCountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<RealEstateWithRequestCountDto>>> GetEncumberedEstates()
    {
        try
        {
            var result = await analyticsService.GetEncumberedEstatesWithRequestsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting encumbered estates");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.Application.Services;
using RealEstateAgencyApp.Application.Dtos.AnalyticsDtos;

namespace RealEstateAgencyApp.API.Controllers;

/// <summary>
/// Analytics endpoints for real estate objects, counterparties, and requests.
/// </summary>
/// <param name="analyticsService">Service that provides analytics operations.</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(AnalyticsService analyticsService) : ControllerBase
{
    /// <summary>
    /// Returns the top counterparties by total request value.
    /// </summary>
    /// <param name="topCount">Number of top counterparties to return (default: 10).</param>
    [HttpGet("top-counterparties")]
    public async Task<ActionResult<List<CounterpartyWithTotalValueDto>>> GetTopCounterparties([FromQuery] int topCount = 10)
    {
        var result = await analyticsService.GetTopCounterpartiesByValueAsync(topCount);
        return Ok(result);
    }

    /// <summary>
    /// Returns the most popular real estate objects by request count.
    /// </summary>
    /// <param name="topCount">Number of popular estates to return (default: 10).</param>
    [HttpGet("popular-estates")]
    public async Task<ActionResult<List<RealEstateWithRequestCountDto>>> GetMostPopularEstates([FromQuery] int topCount = 10)
    {
        var result = await analyticsService.GetMostPopularEstatesAsync(topCount);
        return Ok(result);
    }

    /// <summary>
    /// Returns price statistics by real estate type.
    /// </summary>
    [HttpGet("price-statistics")]
    public async Task<ActionResult<List<PriceStatsByTypeDto>>> GetPriceStatistics()
    {
        var result = await analyticsService.GetPriceStatisticsByTypeAsync();
        return Ok(result);
    }

    /// <summary>
    /// Returns monthly request statistics for the last year.
    /// </summary>
    [HttpGet("monthly-statistics")]
    public async Task<ActionResult<List<MonthlyRequestsDto>>> GetMonthlyStatistics()
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddYears(-1);
        var result = await analyticsService.GetMonthlyStatisticsAsync(startDate, endDate);
        return Ok(result);
    }

    /// <summary>
    /// Returns real estate objects with encumbrances that have active requests.
    /// </summary>
    [HttpGet("encumbered-estates")]
    public async Task<ActionResult<List<RealEstateWithRequestCountDto>>> GetEncumberedEstates()
    {
        var result = await analyticsService.GetEncumberedEstatesWithRequestsAsync();
        return Ok(result);
    }
}
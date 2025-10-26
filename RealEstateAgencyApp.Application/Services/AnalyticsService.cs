using AutoMapper;
using RealEstateAgencyApp.Contracts.Dtos.AnalyticsDtos;
using RealEstateAgencyApp.Contracts.Interfaces;
using RealEstateAgencyApp.Domain.Interfaces;

namespace RealEstateAgencyApp.Application.Services;

/// <summary>
/// Provides analytics operations related to real estate objects, counterparties, and requests.
/// </summary>
/// <param name="requestRepository">Repository for accessing requests.</param>
/// <param name="realEstateRepository">Repository for accessing real estate objects.</param>
/// <param name="mapper">Mapper for DTOs.</param>
public class AnalyticsService(
    IRequestRepository requestRepository,
    IRealEstateObjectRepository realEstateRepository,
    IMapper mapper
) : IAnalyticsService
{
    /// <summary>
    /// Returns the top counterparties by total request value.
    /// </summary>
    /// <param name="topCount">Number of top counterparties to return.</param>
    /// <returns>List of counterparties with their total request values.</returns>
    public async Task<List<CounterpartyWithTotalValueDto>> GetTopCounterpartiesByValueAsync(int topCount = 10)
    {
        var requests = await requestRepository.GetAllAsync();

        var topCounterparties = requests
            .GroupBy(r => r.Counterparty.Id)
            .Select(g =>
            {
                var counterparty = g.First().Counterparty;
                var dto = mapper.Map<CounterpartyWithTotalValueDto>(counterparty);
                dto.TotalValue = g.Sum(r => r.Price);
                return dto;
            })
            .OrderByDescending(c => c.TotalValue)
            .Take(topCount)
            .ToList();

        return topCounterparties;
    }

    /// <summary>
    /// Returns the most popular real estate objects by request count.
    /// </summary>
    /// <param name="topCount">Number of popular estates to return.</param>
    /// <returns>List of real estate objects with their request counts.</returns>
    public async Task<List<RealEstateWithRequestCountDto>> GetMostPopularEstatesAsync(int topCount = 10)
    {
        var requests = await requestRepository.GetAllAsync();

        var popularEstates = requests
            .GroupBy(r => r.Estate.Id)
            .Select(g =>
            {
                var estate = g.First().Estate;
                var dto = mapper.Map<RealEstateWithRequestCountDto>(estate);
                dto.RequestCount = g.Count();
                return dto;
            })
            .OrderByDescending(e => e.RequestCount)
            .Take(topCount)
            .ToList();

        return popularEstates;
    }

    /// <summary>
    /// Returns price statistics by real estate type.
    /// </summary>
    /// <returns>List of price statistics for each real estate type.</returns>
    public async Task<List<PriceStatsByTypeDto>> GetPriceStatisticsByTypeAsync()
    {
        var requests = await requestRepository.GetAllAsync();

        var statsByType = requests
            .GroupBy(r => r.Estate.Type)
            .Select(g => new PriceStatsByTypeDto
            {
                Type = g.Key,
                AveragePrice = g.Average(r => r.Price),
                MaxPrice = g.Max(r => r.Price),
                MinPrice = g.Min(r => r.Price),
                RequestCount = g.Count()
            })
            .OrderByDescending(s => s.RequestCount)
            .ToList();

        return statsByType;
    }

    /// <summary>
    /// Returns monthly request statistics.
    /// </summary>
    /// <param name="startDate">Start date of the period.</param>
    /// <param name="endDate">End date of the period.</param>
    /// <returns>List of monthly request statistics.</returns>
    public async Task<List<MonthlyRequestsDto>> GetMonthlyStatisticsAsync(DateTime startDate, DateTime endDate)
    {
        var requests = await requestRepository.GetAllAsync();

        var monthlyStats = requests
            .Where(r => r.Date >= startDate && r.Date <= endDate)
            .GroupBy(r => new { r.Date.Year, r.Date.Month })
            .Select(g => new MonthlyRequestsDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                BuyCount = g.Count(r => r.Type == Domain.Entities.Enums.RequestType.Buy),
                SellCount = g.Count(r => r.Type == Domain.Entities.Enums.RequestType.Sell),
                TotalCount = g.Count(),
                TotalValue = g.Sum(r => r.Price)
            })
            .OrderBy(m => m.Month)
            .ToList();

        return monthlyStats;
    }

    /// <summary>
    /// Returns real estate objects with encumbrances that have active requests.
    /// </summary>
    /// <returns>List of encumbered estates with request counts.</returns>
    public async Task<List<RealEstateWithRequestCountDto>> GetEncumberedEstatesWithRequestsAsync()
    {
        var requests = await requestRepository.GetAllAsync();
        var estates = await realEstateRepository.GetAllAsync();

        var encumberedEstates = requests
            .Where(r => r.Estate.HasEncumbrances == true)
            .GroupBy(r => r.Estate.Id)
            .Select(g =>
            {
                var estate = g.First().Estate;
                var dto = mapper.Map<RealEstateWithRequestCountDto>(estate);
                dto.RequestCount = g.Count();
                return dto;
            })
            .OrderByDescending(e => e.RequestCount)
            .ToList();

        return encumberedEstates;
    }
}
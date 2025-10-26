using RealEstateAgencyApp.Contracts.Dtos.AnalyticsDtos;

namespace RealEstateAgencyApp.Contracts.Interfaces;

public interface IAnalyticsService
{
    public Task<List<CounterpartyWithTotalValueDto>> GetTopCounterpartiesByValueAsync(int topCount = 10);
    public Task<List<RealEstateWithRequestCountDto>> GetMostPopularEstatesAsync(int topCount = 10);
    public Task<List<PriceStatsByTypeDto>> GetPriceStatisticsByTypeAsync();
    public Task<List<MonthlyRequestsDto>> GetMonthlyStatisticsAsync(DateTime startDate, DateTime endDate);
    public Task<List<RealEstateWithRequestCountDto>> GetEncumberedEstatesWithRequestsAsync();
}
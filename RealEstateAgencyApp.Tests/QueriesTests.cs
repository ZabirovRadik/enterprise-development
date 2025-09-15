using System.Data;
using RealEstateAgencyApp.Tests.Seeds;

namespace RealEstateAgencyApp.Tests;

/// <summary>
/// Contains unit tests for verifying queries on seeded real estate data.
/// Uses DataSeed as a shared fixture to provide test data.
/// </summary>
public class QueriesTests : IClassFixture<DataSeed>
{
    private readonly DataSeed _testData;

    /// <summary>
    /// Initializes a new instance of the QueriesTests class
    /// with the provided DataSeed fixture.
    /// </summary>
    /// <param name="testData">The seeded data used for running queries in tests.</param>
    public QueriesTests(DataSeed testData)
    {
        _testData = testData;
    }

    /// <summary>
    /// Get all sellers who submitted requests in a given date range.
    /// </summary>
    [Fact]
    public void GetSellersByPeriod()
    {
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 12, 31);
        var expected = new[]{"Ivan Ivanov","Sergey Sidorov", "Elena Volkova","Maria Kuznetsova"};

        var sellers = _testData.Requests
            .Where(r => r.Type == RequestType.Sell && r.Date >= from && r.Date <= to)
            .Select(r => r.Counterparty!.FullName)
            .Distinct()
            .Order()
            .ToList();

        Assert.Equal(expected, sellers);
    }

    /// <summary>
    /// Get top 5 clients by number of requests (separately for buy and sell).
    /// </summary>
    [Fact]
    public void GetTop5ClientsByRequests()
    {
        const int expectedTopCount = 5;
        const int expectedRequestCountPerClient = 1;

        var topBuyers = _testData.Requests
            .Where(r => r.Type == RequestType.Buy)
            .GroupBy(r => r.Counterparty)
            .Select(g => new { Client = g.Key!, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        var topSellers = _testData.Requests
            .Where(r => r.Type == RequestType.Sell)
            .GroupBy(r => r.Counterparty)
            .Select(g => new { Client = g.Key!, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.All(topBuyers, x => Assert.Equal(expectedRequestCountPerClient, x.Count));
        Assert.All(topSellers, x => Assert.Equal(expectedRequestCountPerClient, x.Count));
        Assert.Equal(expectedTopCount, topBuyers.Count); 
        Assert.Equal(expectedTopCount, topSellers.Count);
    }

    /// <summary>
    /// Get request count for each type of real estate.
    /// </summary>
    [Fact]
    public void GetRequestCountByRealEstateType()
    {
        const int expectedApartments = 3; // Id 1,6,9
        const int expectedHouses = 2;     // Id 2,7
        const int expectedOffices = 2;    // Id 3,8
        const int expectedLands = 2;      // Id 4,10
        const int expectedGarages = 1;    // Id 5

        var stats = _testData.Requests
            .GroupBy(r => r.Estate!.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToDictionary(x => x.Type, x => x.Count);

        Assert.Equal(expectedApartments, stats[RealEstateType.Apartment]);
        Assert.Equal(expectedHouses, stats[RealEstateType.House]);
        Assert.Equal(expectedOffices, stats[RealEstateType.Office]);
        Assert.Equal(expectedLands, stats[RealEstateType.Land]);
        Assert.Equal(expectedGarages, stats[RealEstateType.Garage]);
    }

    /// <summary>
    /// Get clients who opened requests with the minimum price.
    /// </summary>
    [Fact]
    public void GetClientsWithMinPriceRequests()
    {
        const int expectedMinPrice = 600_000;
        var expectedClient = new[] { "Elena Volkova" };

        var minPrice = _testData.Requests.Min(r => r.Price);

        var clients = _testData.Requests
            .Where(r => r.Price == minPrice)
            .Select(r => r.Counterparty!.FullName)
            .Distinct()
            .ToList();

        Assert.Equal(expectedMinPrice, minPrice);
        Assert.Equal(expectedClient, clients);
    }

    /// <summary>
    /// Get clients looking for a specific real estate type, ordered by name.
    /// </summary>
    [Fact]
    public void GetClientsByEstateType()
    {
        const RealEstateType targetType = RealEstateType.Apartment;
        var expectedClients = new[] { "Dmitry Orlov" };

        var clients = _testData.Requests
            .Where(r => r.Type == RequestType.Buy && r.Estate!.Type == targetType)
            .Select(r => r.Counterparty!.FullName)
            .Distinct()
            .Order()
            .ToList();

        Assert.Equal(expectedClients, clients);
    }
}

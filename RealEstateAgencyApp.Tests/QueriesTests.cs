using System.Data;

namespace RealEstateAgencyApp.Tests;

public class QueriesTests
{
    private readonly List<Counterparty> _counterparties;
    private readonly List<RealEstateObject> _objects;
    private readonly List<Request> _requests;

    public QueriesTests()
    {
        _counterparties = DataSeed.GetClients();
        _objects = DataSeed.GetEstates();
        _requests = DataSeed.GetRequests(_counterparties, _objects);
    }

    /// <summary>
    /// Get all sellers who submitted requests in a given date range.
    /// </summary>
    [Fact]
    public void GetSellersByPeriod()
    {
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 12, 31);

        var sellers = _requests
            .Where(r => r.Type == RequestType.Sell && r.Date >= from && r.Date <= to)
            .Select(r => r.Counterparty!.FullName)
            .Distinct()
            .ToList();

        // expecting Id=1 (Ivanov), 3 (Sidorov), 5 (Volkova), 7 (Kuznetsova)
        var expected = new[] { "Ivan Ivanov", "Sergey Sidorov", "Elena Volkova", "Maria Kuznetsova" };

        Assert.Equal(expected.OrderBy(x => x), sellers.OrderBy(x => x));
    }

    /// <summary>
    /// Get top 5 clients by number of requests (separately for buy and sell).
    /// </summary>
    [Fact]
    public void GetTop5ClientsByRequests()
    {
        var topBuyers = _requests
            .Where(r => r.Type == RequestType.Buy)
            .GroupBy(r => r.Counterparty)
            .Select(g => new { Client = g.Key!, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        var topSellers = _requests
            .Where(r => r.Type == RequestType.Sell)
            .GroupBy(r => r.Counterparty)
            .Select(g => new { Client = g.Key!, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.All(topBuyers, x => Assert.Equal(1, x.Count));
        Assert.All(topSellers, x => Assert.Equal(1, x.Count));
        Assert.Equal(5, topBuyers.Count); 
        Assert.Equal(5, topSellers.Count);
    }

    /// <summary>
    /// Get request count for each type of real estate.
    /// </summary>
    [Fact]
    public void GetRequestCountByRealEstateType()
    {
        var stats = _requests
            .GroupBy(r => r.Estate!.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToDictionary(x => x.Type, x => x.Count);

        Assert.Equal(3, stats[RealEstateType.Apartment]); // Id 1,6,9
        Assert.Equal(2, stats[RealEstateType.House]);     // Id 2,7
        Assert.Equal(2, stats[RealEstateType.Office]);    // Id 3,8
        Assert.Equal(2, stats[RealEstateType.Land]);      // Id 4,10
        Assert.Equal(1, stats[RealEstateType.Garage]);    // Id 5
    }

    /// <summary>
    /// Get clients who opened requests with the minimum price.
    /// </summary>
    [Fact]
    public void GetClientsWithMinPriceRequests()
    {
        var minPrice = _requests.Min(r => r.Price);

        var clients = _requests
            .Where(r => r.Price == minPrice)
            .Select(r => r.Counterparty!.FullName)
            .Distinct()
            .ToList();

        // minimal cost = 600 000 (request 5, client = Elena Volkova)
        Assert.Single(clients);
        Assert.Equal("Elena Volkova", clients[0]);
    }

    /// <summary>
    /// Get clients looking for a specific real estate type, ordered by name.
    /// </summary>
    [Fact]
    public void GetClientsByEstateType()
    {
        var type = RealEstateType.Apartment;

        var clients = _requests
            .Where(r => r.Type == RequestType.Buy && r.Estate!.Type == type)
            .Select(r => r.Counterparty!.FullName)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        // from seed: buying an apartment only from Dmitry Orlov (request 6)
        Assert.Single(clients);
        Assert.Equal("Dmitry Orlov", clients[0]);
    }
}

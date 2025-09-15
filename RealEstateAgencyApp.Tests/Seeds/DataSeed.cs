namespace RealEstateAgencyApp.Tests.Seeds;

/// <summary>
/// Provides initial data for seeding in-memory collections.
/// </summary>
public class DataSeed
{
    /// <summary>
    /// Gets the list of real estate objects.
    /// </summary>
    public List<RealEstateObject> Estates { get; }

    /// <summary>
    /// Gets the list of counterparties (clients).
    /// </summary>
    public List<Counterparty> Clients { get; }

    /// <summary>
    /// Gets the list of requests linking clients with estates.
    /// </summary>
    public List<Request> Requests { get; }

    /// <summary>
    /// Initializes seed data collections.
    /// </summary>
    public DataSeed()
    {
        Estates = InitEstates();
        Clients = InitClients();
        Requests = InitRequests(Clients, Estates);
    }

    /// <summary>
    /// Initializes a predefined list of real estate objects.
    /// </summary>
    private List<RealEstateObject> InitEstates() =>
        new()
        {
            new RealEstateObject { Id = 1, Type = RealEstateType.Apartment, Purpose = RealEstatePurpose.Residential, CadastralNumber="77:01:0001", Address="Moscow, Lenina St. 1", Floors=10, Area=50, Rooms=2, CeilingHeight=2.7, Floor=5 },
            new RealEstateObject { Id = 2, Type = RealEstateType.House, Purpose = RealEstatePurpose.Residential, CadastralNumber="77:01:0002", Address="Moscow region, Sosnovaya 3", Floors=2, Area=120, Rooms=4, CeilingHeight=3.0, Floor=1 },
            new RealEstateObject { Id = 3, Type = RealEstateType.Office, Purpose = RealEstatePurpose.Commercial, CadastralNumber="77:01:0003", Address="Moscow, Business Center 12", Floors=15, Area=200, Rooms=10, CeilingHeight=3.2, Floor=7},
            new RealEstateObject { Id = 4, Type = RealEstateType.Land, Purpose = RealEstatePurpose.Residential, CadastralNumber="77:01:0004", Address="Moscow region, Greenfield",Area=600},
            new RealEstateObject { Id = 5, Type = RealEstateType.Garage, Purpose = RealEstatePurpose.Commercial, CadastralNumber="77:01:0005", Address="Moscow, Garage Cooperative 21", Floors=1, Area=25, Rooms=1, CeilingHeight=2.5},
            new RealEstateObject { Id = 6, Type = RealEstateType.Apartment, Purpose = RealEstatePurpose.Residential, CadastralNumber="77:01:0006", Address="Moscow, Pushkina St. 10", Floors=12, Area=75, Rooms=3, CeilingHeight=2.8, Floor=9 },
            new RealEstateObject { Id = 7, Type = RealEstateType.House, Purpose = RealEstatePurpose.Residential, CadastralNumber="77:01:0007", Address="Moscow region, Central St. 55", Floors=3, Area=200, Rooms=6, CeilingHeight=3.1, Floor=1, HasEncumbrances=true },
            new RealEstateObject { Id = 8, Type = RealEstateType.Office, Purpose = RealEstatePurpose.Commercial, CadastralNumber="77:01:0008", Address="Moscow, Office Park 8", Floors=20, Area=300, Rooms=15, CeilingHeight=3.3, Floor=10 },
            new RealEstateObject { Id = 9, Type = RealEstateType.Apartment, Purpose = RealEstatePurpose.Residential, CadastralNumber="77:01:0009", Address="Moscow, Tverskaya St. 15", Floors=8, Area=60, Rooms=2, CeilingHeight=2.6, Floor=3 },
            new RealEstateObject { Id = 10, Type = RealEstateType.Land, Purpose = RealEstatePurpose.Commercial, CadastralNumber="77:01:0010", Address="Moscow region, Industrial Zone", Area=1500, HasEncumbrances=true }
        };

    /// <summary>
    /// Initializes a predefined list of counterparties (clients).
    /// </summary>
    private List<Counterparty> InitClients() =>
        new()
        {
            new Counterparty { Id = 1, FullName="Ivan Ivanov", PassportNumber="4500 123456", Phone="+7 999 111-22-33" },
            new Counterparty { Id = 2, FullName="Petr Petrov", PassportNumber="4500 654321", Phone="+7 999 444-55-66" },
            new Counterparty { Id = 3, FullName="Sergey Sidorov", PassportNumber="4500 222333", Phone="+7 999 777-88-99" },
            new Counterparty { Id = 4, FullName="Anna Smirnova", PassportNumber="4500 444555", Phone="+7 999 000-11-22" },
            new Counterparty { Id = 5, FullName="Elena Volkova", PassportNumber="4500 666777", Phone="+7 999 333-44-55" },
            new Counterparty { Id = 6, FullName="Dmitry Orlov", PassportNumber="4500 888999", Phone="+7 999 555-66-77" },
            new Counterparty { Id = 7, FullName="Maria Kuznetsova", PassportNumber="4500 101112", Phone="+7 999 888-99-00" },
            new Counterparty { Id = 8, FullName="Alexey Romanov", PassportNumber="4500 131415", Phone="+7 999 222-33-44" },
            new Counterparty { Id = 9, FullName="Natalia Ivanova", PassportNumber="4500 161718", Phone="+7 999 666-77-88" },
            new Counterparty { Id = 10, FullName="Andrey Popov", PassportNumber="4500 192021", Phone="+7 999 999-00-11" }
        };

    /// <summary>
    /// Initializes a predefined list of requests.
    /// </summary>
    private List<Request> InitRequests(List<Counterparty> clients, List<RealEstateObject> estates) =>
        new()
        {
            new Request { Id = 1,  Counterparty=clients[0],  Estate=estates[0], Type=RequestType.Sell, Price=8_000_000, Date=new DateTime(2024,5,10) },
            new Request { Id = 2, Counterparty=clients[1], Estate=estates[1], Type=RequestType.Buy, Price=12_500_000, Date=new DateTime(2024,6,15) },
            new Request { Id = 3, Counterparty=clients[2], Estate=estates[2], Type=RequestType.Sell, Price=45_000_000, Date=new DateTime(2024,7,01) },
            new Request { Id = 4, Counterparty=clients[3], Estate=estates[3], Type=RequestType.Buy, Price=3_000_000, Date=new DateTime(2024,8,12) },
            new Request { Id = 5, Counterparty=clients[4], Estate=estates[4], Type=RequestType.Sell, Price=600_000, Date=new DateTime(2024,9,20) },
            new Request { Id = 6, Counterparty=clients[5], Estate=estates[5], Type=RequestType.Buy, Price=10_000_000, Date=new DateTime(2024,10,05) },
            new Request { Id = 7, Counterparty=clients[6], Estate=estates[6], Type=RequestType.Sell, Price=25_000_000, Date=new DateTime(2024,11,11) },
            new Request { Id = 8, Counterparty=clients[7], Estate=estates[7], Type=RequestType.Buy, Price=55_000_000, Date=new DateTime(2024,12,01) },
            new Request { Id = 9, Counterparty=clients[8], Estate=estates[8], Type=RequestType.Sell, Price=9_000_000, Date=new DateTime(2025,1,15) },
            new Request { Id = 10, Counterparty=clients[9], Estate=estates[9], Type=RequestType.Buy, Price=80_000_000, Date=new DateTime(2025,2,20) }
        };
}

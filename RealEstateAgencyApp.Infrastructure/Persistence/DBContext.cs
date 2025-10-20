using Microsoft.EntityFrameworkCore;
using RealEstateAgencyApp.Domain.Entities;

namespace RealEstateAgencyApp.Infrastructure.Persistence;

/// <summary>
/// EF Core database context for the real estate agency application.
/// </summary>
/// <param name="options">The options for this context.</param>
public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    /// <summary>
    /// DbSet of real estate objects in the agency.
    /// </summary>
    public DbSet<RealEstateObject> RealEstateObjects { get; set; }

    /// <summary>
    /// DbSet of counterparties (clients) in the the agency.
    /// </summary>
    public DbSet<Counterparty> Counterparties { get; set; }

    /// <summary>
    /// DbSet of requests, representing buy/sell requests from counterparties.
    /// </summary>
    public DbSet<Request> Requests { get; set; }

    /// <summary>
    /// Configures the EF Core model.
    /// </summary>
    /// <param name="modelBuilder">Model builder instance.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Counterparty>(c =>
        {
            c.HasKey(c => c.Id);
            c.Property(c => c.Id)
                .ValueGeneratedOnAdd();
            c.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(128);
            c.Property(c => c.PassportNumber)
                .IsRequired()
                .HasMaxLength(20);
            c.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(20);
            c.HasIndex(c => c.PassportNumber)
                .IsUnique();
        });

        modelBuilder.Entity<RealEstateObject>(b =>
        {
            b.HasKey(b => b.Id);
            b.Property(b => b.Id)
                .ValueGeneratedOnAdd();
            b.Property(b => b.CadastralNumber)
                .IsRequired()
                .HasMaxLength(30);
            b.Property(b => b.Address)
               .IsRequired()
               .HasMaxLength(256);
            b.Property(b => b.Area)
               .IsRequired()
               .HasColumnType("double");
            b.Property(b => b.Floors)
               .IsRequired(false);
            b.Property(b => b.Rooms)
               .IsRequired(false);
            b.Property(b => b.CeilingHeight)
               .IsRequired(false)
               .HasColumnType("double");
            b.Property(b => b.Floor)
               .IsRequired(false);
            b.Property(b => b.HasEncumbrances)
               .IsRequired(false);
            b.Property(b => b.Type)
               .HasConversion<string>()
               .IsRequired();
            b.Property(b => b.Purpose)
               .HasConversion<string>()
               .IsRequired();
            b.HasIndex(b => b.CadastralNumber)
                .IsUnique();
        });

        modelBuilder.Entity<Request>(b =>
        {
            b.HasKey(b => b.Id);
            b.Property(b => b.Id)
                .ValueGeneratedOnAdd();
            b.Property(b => b.Type)
                .HasConversion<string>()
                .IsRequired();
            b.Property(b => b.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            b.Property(b => b.Date)
                .IsRequired();

            b.HasOne<Counterparty>()
                .WithMany()
                .HasForeignKey(r => r.CounterpartyID)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<RealEstateObject>()
                .WithMany()
                .HasForeignKey(r => r.EstateID)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
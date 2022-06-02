using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APPventureBanking.Models;

public class BankContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<BillingPayee> BillingPayees { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Identity> Identities { get; set; }
    public DbSet<Party> Parties { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public string DbPath { get; }

    public BankContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "bank.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Party>()
            .Property(e => e.Type)
            .HasConversion(
                v => v.ToString(),
                v => (PartyType) Enum.Parse(typeof(PartyType), v));

        modelBuilder
            .Entity<Account>()
            .Property(e => e.AccountType)
            .HasConversion(
                v => v.ToString(),
                v => (AccountType) Enum.Parse(typeof(AccountType), v));
        
        modelBuilder
            .Entity<Card>()
            .Property(e => e.CardType)
            .HasConversion(
                v => v.ToString(),
                v => (CardType) Enum.Parse(typeof(CardType), v));

        var mailingAddress1 = new Address
        {
            AddressId = 1,
            AddressLine1 = "123 Main St",
            City = "Anytown",
            StateCode = "CA",
            PostalCode = "12345"
        };

        var mailingAddress2 = new Address
        {
            AddressId = 2,
            AddressLine1 = "55 Thompson Pl 2nd Floor",
            City = "Boston",
            StateCode = "MA",
            PostalCode = "02210"
        };

        modelBuilder.Entity<Address>().HasData(mailingAddress1, mailingAddress2);
        
        var testParty1 = new Party
        {
            PartyId = 1,
            Type = PartyType.Person,
            FirstName = "John",
            LastName = "Doe",
            PrimaryEmailAddress = "john.doe@gmail.com",
            MailingAddressId = 1
        };
        
        var testParty2 = new Party
        {
            PartyId = 2,
            Type = PartyType.Entity,
            EntityName = "OutSystems",
            PrimaryEmailAddress = "sales@outsystems.com",
            MailingAddressId = 2
        };
        
        modelBuilder.Entity<Party>().HasData(testParty1, testParty2);
        
        modelBuilder.Entity<Identity>()
            .HasData(
                new Identity
                {
                    IdentityId = 1,
                    PartyId = 1
                },
                new Identity
                {
                    IdentityId = 2,
                    PartyId = 2
                });
    }
}

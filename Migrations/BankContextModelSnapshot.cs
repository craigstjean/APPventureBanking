﻿// <auto-generated />
using System;
using APPventureBanking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APPventureBanking.Migrations
{
    [DbContext(typeof(BankContext))]
    partial class BankContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("AccountIdentity", b =>
                {
                    b.Property<int>("AccountsAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdentitiesIdentityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AccountsAccountId", "IdentitiesIdentityId");

                    b.HasIndex("IdentitiesIdentityId");

                    b.ToTable("AccountIdentity");
                });

            modelBuilder.Entity("APPventureBanking.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("APPventureBanking.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StateCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");

                    b.HasData(
                        new
                        {
                            AddressId = 1,
                            AddressLine1 = "123 Main St",
                            City = "Anytown",
                            PostalCode = "12345",
                            StateCode = "CA"
                        },
                        new
                        {
                            AddressId = 2,
                            AddressLine1 = "55 Thompson Pl 2nd Floor",
                            City = "Boston",
                            PostalCode = "02210",
                            StateCode = "MA"
                        });
                });

            modelBuilder.Entity("APPventureBanking.Models.Bill", b =>
                {
                    b.Property<int>("BillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BillingPayeeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CentsDue")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DollarsDue")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("DueDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("IdentityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BillId");

                    b.HasIndex("BillingPayeeId");

                    b.HasIndex("IdentityId");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("APPventureBanking.Models.BillingPayee", b =>
                {
                    b.Property<int>("BillingPayeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BillingAddressId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartyId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReferenceAccountId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BillingPayeeId");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("PartyId");

                    b.HasIndex("ReferenceAccountId");

                    b.ToTable("BillingPayees");
                });

            modelBuilder.Entity("APPventureBanking.Models.Card", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CardType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.HasKey("CardId");

                    b.HasIndex("AccountId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("APPventureBanking.Models.Identity", b =>
                {
                    b.Property<int>("IdentityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartyId")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdentityId");

                    b.HasIndex("PartyId");

                    b.ToTable("Identities");

                    b.HasData(
                        new
                        {
                            IdentityId = 1,
                            PartyId = 1
                        },
                        new
                        {
                            IdentityId = 2,
                            PartyId = 2
                        });
                });

            modelBuilder.Entity("APPventureBanking.Models.Party", b =>
                {
                    b.Property<int>("PartyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EntityName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<int>("MailingAddressId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PrimaryEmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PartyId");

                    b.HasIndex("MailingAddressId");

                    b.ToTable("Parties");

                    b.HasData(
                        new
                        {
                            PartyId = 1,
                            FirstName = "John",
                            LastName = "Doe",
                            MailingAddressId = 1,
                            PrimaryEmailAddress = "john.doe@gmail.com",
                            Type = "Person"
                        },
                        new
                        {
                            PartyId = 2,
                            EntityName = "OutSystems",
                            MailingAddressId = 2,
                            PrimaryEmailAddress = "sales@outsystems.com",
                            Type = "Entity"
                        });
                });

            modelBuilder.Entity("APPventureBanking.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BillId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cents")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Dollars")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FromAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ToAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TransactionDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TransactionId");

                    b.HasIndex("BillId");

                    b.HasIndex("FromAccountId");

                    b.HasIndex("ToAccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("AccountIdentity", b =>
                {
                    b.HasOne("APPventureBanking.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APPventureBanking.Models.Identity", null)
                        .WithMany()
                        .HasForeignKey("IdentitiesIdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("APPventureBanking.Models.Bill", b =>
                {
                    b.HasOne("APPventureBanking.Models.BillingPayee", "BillingPayee")
                        .WithMany()
                        .HasForeignKey("BillingPayeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APPventureBanking.Models.Identity", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BillingPayee");

                    b.Navigation("Identity");
                });

            modelBuilder.Entity("APPventureBanking.Models.BillingPayee", b =>
                {
                    b.HasOne("APPventureBanking.Models.Address", "BillingAddress")
                        .WithMany()
                        .HasForeignKey("BillingAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APPventureBanking.Models.Party", "Party")
                        .WithMany()
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APPventureBanking.Models.Account", "ReferenceAccount")
                        .WithMany()
                        .HasForeignKey("ReferenceAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BillingAddress");

                    b.Navigation("Party");

                    b.Navigation("ReferenceAccount");
                });

            modelBuilder.Entity("APPventureBanking.Models.Card", b =>
                {
                    b.HasOne("APPventureBanking.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("APPventureBanking.Models.Identity", b =>
                {
                    b.HasOne("APPventureBanking.Models.Party", "Party")
                        .WithMany()
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Party");
                });

            modelBuilder.Entity("APPventureBanking.Models.Party", b =>
                {
                    b.HasOne("APPventureBanking.Models.Address", "MailingAddress")
                        .WithMany()
                        .HasForeignKey("MailingAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MailingAddress");
                });

            modelBuilder.Entity("APPventureBanking.Models.Transaction", b =>
                {
                    b.HasOne("APPventureBanking.Models.Bill", null)
                        .WithMany("AssociatedTransactions")
                        .HasForeignKey("BillId");

                    b.HasOne("APPventureBanking.Models.Account", "FromAccount")
                        .WithMany()
                        .HasForeignKey("FromAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APPventureBanking.Models.Account", "ToAccount")
                        .WithMany()
                        .HasForeignKey("ToAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromAccount");

                    b.Navigation("ToAccount");
                });

            modelBuilder.Entity("APPventureBanking.Models.Bill", b =>
                {
                    b.Navigation("AssociatedTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}

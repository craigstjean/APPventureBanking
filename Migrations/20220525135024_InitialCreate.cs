using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APPventureBanking.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressLine1 = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine2 = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    StateCode = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardType = table.Column<string>(type: "TEXT", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpirationDate = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Cards_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    PartyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    EntityName = table.Column<string>(type: "TEXT", nullable: false),
                    PrimaryEmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    MailingAddressAddressId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.PartyId);
                    table.ForeignKey(
                        name: "FK_Parties_Addresses_MailingAddressAddressId",
                        column: x => x.MailingAddressAddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillingPayees",
                columns: table => new
                {
                    BillingPayeeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartyId = table.Column<int>(type: "INTEGER", nullable: false),
                    BillingAddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReferenceAccountId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPayees", x => x.BillingPayeeId);
                    table.ForeignKey(
                        name: "FK_BillingPayees_Accounts_ReferenceAccountId",
                        column: x => x.ReferenceAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillingPayees_Addresses_BillingAddressId",
                        column: x => x.BillingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillingPayees_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identities",
                columns: table => new
                {
                    IdentityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.IdentityId);
                    table.ForeignKey(
                        name: "FK_Identities_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountIdentity",
                columns: table => new
                {
                    AccountsAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    IdentitiesIdentityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountIdentity", x => new { x.AccountsAccountId, x.IdentitiesIdentityId });
                    table.ForeignKey(
                        name: "FK_AccountIdentity_Accounts_AccountsAccountId",
                        column: x => x.AccountsAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountIdentity_Identities_IdentitiesIdentityId",
                        column: x => x.IdentitiesIdentityId,
                        principalTable: "Identities",
                        principalColumn: "IdentityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BillingPayeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DollarsDue = table.Column<int>(type: "INTEGER", nullable: false),
                    CentsDue = table.Column<int>(type: "INTEGER", nullable: false),
                    IdentityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bills_BillingPayees_BillingPayeeId",
                        column: x => x.BillingPayeeId,
                        principalTable: "BillingPayees",
                        principalColumn: "BillingPayeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bills_Identities_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identities",
                        principalColumn: "IdentityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FromAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionType = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Dollars = table.Column<int>(type: "INTEGER", nullable: false),
                    Cents = table.Column<int>(type: "INTEGER", nullable: false),
                    BillId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_FromAccountId",
                        column: x => x.FromAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_ToAccountId",
                        column: x => x.ToAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "BillId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountIdentity_IdentitiesIdentityId",
                table: "AccountIdentity",
                column: "IdentitiesIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPayees_BillingAddressId",
                table: "BillingPayees",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPayees_PartyId",
                table: "BillingPayees",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPayees_ReferenceAccountId",
                table: "BillingPayees",
                column: "ReferenceAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BillingPayeeId",
                table: "Bills",
                column: "BillingPayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_IdentityId",
                table: "Bills",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AccountId",
                table: "Cards",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Identities_PartyId",
                table: "Identities",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_MailingAddressAddressId",
                table: "Parties",
                column: "MailingAddressAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BillId",
                table: "Transactions",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromAccountId",
                table: "Transactions",
                column: "FromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToAccountId",
                table: "Transactions",
                column: "ToAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountIdentity");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "BillingPayees");

            migrationBuilder.DropTable(
                name: "Identities");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}

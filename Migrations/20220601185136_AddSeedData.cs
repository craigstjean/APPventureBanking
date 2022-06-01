using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APPventureBanking.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressId", "AddressLine1", "AddressLine2", "City", "PostalCode", "StateCode" },
                values: new object[] { 1, "123 Main St", null, "Anytown", "12345", "CA" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressId", "AddressLine1", "AddressLine2", "City", "PostalCode", "StateCode" },
                values: new object[] { 2, "55 Thompson Pl 2nd Floor", null, "Boston", "02210", "MA" });

            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "PartyId", "EntityName", "FirstName", "LastName", "MailingAddressId", "PrimaryEmailAddress", "Type" },
                values: new object[] { 1, null, "John", "Doe", 1, "john.doe@gmail.com", "Person" });

            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "PartyId", "EntityName", "FirstName", "LastName", "MailingAddressId", "PrimaryEmailAddress", "Type" },
                values: new object[] { 2, "OutSystems", null, null, 2, "sales@outsystems.com", "Entity" });

            migrationBuilder.InsertData(
                table: "Identities",
                columns: new[] { "IdentityId", "PartyId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Identities",
                columns: new[] { "IdentityId", "PartyId" },
                values: new object[] { 2, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Identities",
                keyColumn: "IdentityId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Identities",
                keyColumn: "IdentityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Parties",
                keyColumn: "PartyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parties",
                keyColumn: "PartyId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "AddressId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "AddressId",
                keyValue: 2);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APPventureBanking.Migrations
{
    public partial class UpdateNullableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Addresses_MailingAddressAddressId",
                table: "Parties");

            migrationBuilder.RenameColumn(
                name: "MailingAddressAddressId",
                table: "Parties",
                newName: "MailingAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Parties_MailingAddressAddressId",
                table: "Parties",
                newName: "IX_Parties_MailingAddressId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Parties",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Parties",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "EntityName",
                table: "Parties",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                table: "Addresses",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Addresses_MailingAddressId",
                table: "Parties",
                column: "MailingAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Addresses_MailingAddressId",
                table: "Parties");

            migrationBuilder.RenameColumn(
                name: "MailingAddressId",
                table: "Parties",
                newName: "MailingAddressAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Parties_MailingAddressId",
                table: "Parties",
                newName: "IX_Parties_MailingAddressAddressId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Parties",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Parties",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntityName",
                table: "Parties",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                table: "Addresses",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Addresses_MailingAddressAddressId",
                table: "Parties",
                column: "MailingAddressAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

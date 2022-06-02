using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APPventureBanking.Migrations
{
    public partial class CombineAmountFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cents",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Dollars",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CentsDue",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "DollarsDue",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "Transactions",
                newName: "Amount");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountDue",
                table: "Bills",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountDue",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transactions",
                newName: "TransactionType");

            migrationBuilder.AddColumn<int>(
                name: "Cents",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dollars",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CentsDue",
                table: "Bills",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DollarsDue",
                table: "Bills",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}

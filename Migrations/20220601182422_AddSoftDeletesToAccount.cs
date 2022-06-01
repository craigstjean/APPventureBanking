using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APPventureBanking.Migrations
{
    public partial class AddSoftDeletesToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Accounts");
        }
    }
}

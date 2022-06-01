using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APPventureBanking.Migrations
{
    public partial class AddNameToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Accounts");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AccountColumnTypoFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Panronimic", "Accounts", "Patronimic");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Patronimic", "Accounts", "Panronimic");
        }
    }
}

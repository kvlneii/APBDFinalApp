using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APBDBlazorApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class CompanyTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "Company",
                newName: "Website");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Company",
                newName: "City");
        }
    }
}

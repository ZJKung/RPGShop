using Microsoft.EntityFrameworkCore.Migrations;

namespace CatalogApi.Migrations
{
    public partial class updateCatalogItempricefullnametopicturefillname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceFullName",
                table: "Catalog");

            migrationBuilder.AddColumn<string>(
                name: "PictureFileName",
                table: "Catalog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureFileName",
                table: "Catalog");

            migrationBuilder.AddColumn<string>(
                name: "PriceFullName",
                table: "Catalog",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

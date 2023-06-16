using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazinWebLicenta.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProductSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { 1, "Peugeot 208 este un automobil de clasă mică (segment B european) produs de constructorul francez de automobile Peugeot. ", "https://wallpapercave.com/wp/wp4243619.jpg", 4899m, "Peugeot 208" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

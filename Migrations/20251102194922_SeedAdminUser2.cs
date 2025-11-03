using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocOn.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "$2b$10$4hSe9mGQEjgdJxkTez2gbu7eRJ0ghBT0lSZ55pWjxyxSKg5/7/YNS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "$2b$10$5fwCaVKEtlovaha2i4xhROQX5AR5ysu1pdWTzL5O5D7WclN17CDWm");
        }
    }
}

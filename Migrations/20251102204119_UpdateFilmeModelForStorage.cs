using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocOn.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFilmeModelForStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaminhoImagem",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Filmes");

            migrationBuilder.AddColumn<string>(
                name: "UrlCartaz",
                table: "Filmes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlCartaz",
                table: "Filmes");

            migrationBuilder.AddColumn<string>(
                name: "CaminhoImagem",
                table: "Filmes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagem",
                table: "Filmes",
                type: "longblob",
                nullable: true);
        }
    }
}

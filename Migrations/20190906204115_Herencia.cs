using Microsoft.EntityFrameworkCore.Migrations;

namespace Capi.Migrations
{
    public partial class Herencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "detalles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "enpaqueteria",
                table: "detalles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sinenviar",
                table: "detalles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "detalles");

            migrationBuilder.DropColumn(
                name: "enpaqueteria",
                table: "detalles");

            migrationBuilder.DropColumn(
                name: "sinenviar",
                table: "detalles");
        }
    }
}

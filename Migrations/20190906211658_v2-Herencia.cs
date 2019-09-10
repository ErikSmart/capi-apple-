using Microsoft.EntityFrameworkCore.Migrations;

namespace Capi.Migrations
{
    public partial class v2Herencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalles_clientes_clienteId",
                table: "detalles");

            migrationBuilder.AlterColumn<int>(
                name: "clienteId",
                table: "detalles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_detalles_clientes_clienteId",
                table: "detalles",
                column: "clienteId",
                principalTable: "clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalles_clientes_clienteId",
                table: "detalles");

            migrationBuilder.AlterColumn<int>(
                name: "clienteId",
                table: "detalles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_detalles_clientes_clienteId",
                table: "detalles",
                column: "clienteId",
                principalTable: "clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

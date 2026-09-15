using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class Activo_Unidades_Categorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "activo",
                table: "Unidades",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "activo",
                table: "Categorias",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 1,
                column: "activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 2,
                column: "activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 3,
                column: "activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 4,
                column: "activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 5,
                column: "activo",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activo",
                table: "Unidades");

            migrationBuilder.DropColumn(
                name: "activo",
                table: "Categorias");
        }
    }
}

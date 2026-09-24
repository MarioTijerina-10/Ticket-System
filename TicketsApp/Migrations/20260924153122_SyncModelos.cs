using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "contraseña_visible",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Departamentos",
                columns: new[] { "id", "activo", "nombre" },
                values: new object[] { 1, true, "Sistemas" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "contraseña_visible",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Unidades",
                columns: new[] { "id", "activo", "nombre" },
                values: new object[,]
                {
                    { 2, true, "BDF" },
                    { 3, true, "DPC" },
                    { 4, true, "Energy Point" },
                    { 5, true, "Orga" }
                });
        }
    }
}

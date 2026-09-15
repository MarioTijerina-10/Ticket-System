using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class TiempoYValoresAñadidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
    name: "duracion",
    table: "tickets");

            migrationBuilder.AddColumn<int>(
                name: "duracion",
                table: "tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, fecha_inicio, fecha_cierre)");

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "id", "nombre" },
                values: new object[,]
                {
                    { 1, "Software" },
                    { 2, "Hardware" },
                    { 3, "Sistemas ERP" },
                    { 4, "Red e Internet" },
                    { 5, "Impresoras" }
                });

            migrationBuilder.InsertData(
                table: "Estatus",
                columns: new[] { "id", "nombre" },
                values: new object[,]
                {
                    { 1, "Abierto" },
                    { 2, "En Proceso" },
                    { 3, "Cerrado Parcial" },
                    { 4, "Cerrado" }
                });

            migrationBuilder.InsertData(
                table: "Unidades",
                columns: new[] { "id", "nombre" },
                values: new object[,]
                {
                    { 1, "Cyrburgos" },
                    { 2, "BDF" },
                    { 3, "DPC" },
                    { 4, "Energy Point" },
                    { 5, "Orga" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Estatus",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Estatus",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Estatus",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Estatus",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Unidades",
                keyColumn: "id",
                keyValue: 1);

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "duracion",
                table: "Tickets",
                type: "datetime",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, fecha_inicio, fecha_cierre)",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComputedColumnSql: "DATEDIFF(MINUTE, fecha_inicio, fecha_cierre)");
        }
    }
}

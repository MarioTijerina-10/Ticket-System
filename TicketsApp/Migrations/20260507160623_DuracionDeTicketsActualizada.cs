using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class DuracionDeTicketsActualizada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "duracion",
                table: "Tickets",
                type: "datetime",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, fecha_inicio, fecha_cierre)",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "duracion",
                table: "Tickets",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldComputedColumnSql: "DATEDIFF(MINUTE, fecha_inicio, fecha_cierre)");
        }
    }
}

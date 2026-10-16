using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class FirstProductionMigrtion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fecha_cierre",
                table: "Plantillas");

            migrationBuilder.DropColumn(
                name: "fecha_tomado",
                table: "Plantillas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_cierre",
                table: "Plantillas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_tomado",
                table: "Plantillas",
                type: "datetime2",
                nullable: true);
        }
    }
}

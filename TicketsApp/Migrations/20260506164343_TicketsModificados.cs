using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class TicketsModificados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AsignadoAProvedor",
                table: "Tickets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaCierreParcial",
                table: "Tickets",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaTomado",
                table: "Tickets",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provedor",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsignadoAProvedor",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FechaCierreParcial",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FechaTomado",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Provedor",
                table: "Tickets");
        }
    }
}

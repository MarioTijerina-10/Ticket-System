using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class TiemposDeTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TiempoAbierto",
                table: "Tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, fecha_inicio, FechaTomado)");

            migrationBuilder.AddColumn<int>(
                name: "TiempoHastaCierreParcial",
                table: "Tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, FechaTomado, FechaCierreParcial)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TiempoAbierto",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TiempoHastaCierreParcial",
                table: "Tickets");
        }
    }
}

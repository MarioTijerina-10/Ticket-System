using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class TiemposYCambios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TiempoHastaCierre",
                table: "Tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierre)",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComputedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, FechaCierre)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TiempoHastaCierre",
                table: "Tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, FechaCierre)",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComputedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierre)");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class TiemposYCambiosFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TiempoHastaCierre",
                table: "Tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierre   )",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComputedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierre)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                oldComputedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierre   )");
        }
    }
}

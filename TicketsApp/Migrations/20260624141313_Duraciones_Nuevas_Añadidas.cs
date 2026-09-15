using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class Duraciones_Nuevas_Añadidas : Migration
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
                oldComputedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierrex   )");

            migrationBuilder.AddColumn<int>(
                name: "Duracion_Abierto_Cierre",
                table: "Tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, fecha_inicio, fecha_cierre)");

            migrationBuilder.AddColumn<int>(
                name: "Duracion_Abierto_CierreParcial",
                table: "Tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, fecha_inicio, FechaCierreParcial)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duracion_Abierto_Cierre",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Duracion_Abierto_CierreParcial",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "TiempoHastaCierre",
                table: "Tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierrex   )",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComputedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierre   )");
        }
    }
}

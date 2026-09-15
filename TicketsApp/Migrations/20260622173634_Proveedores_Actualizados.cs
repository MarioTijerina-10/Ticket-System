using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class Proveedores_Actualizados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Provedor",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Provedor",
                table: "Tickets",
                column: "Provedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Proveedores",
                table: "Tickets",
                column: "Provedor",
                principalTable: "Proveedor",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Proveedores",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_Provedor",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "Provedor",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class ObservacionesYTiempos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "cerrado_por_usuario",
                table: "Tickets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TiempoHastaCierre",
                table: "Tickets",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierre)");

            migrationBuilder.CreateTable(
                name: "ComentarioTicket",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fk_ticket = table.Column<int>(type: "int", nullable: false),
                    fk_usuario = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentarioTicket", x => x.id);
                    table.ForeignKey(
                        name: "FK_ComentarioTicket_AspNetUsers_fk_usuario",
                        column: x => x.fk_usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ComentarioTicket_Tickets_fk_ticket",
                        column: x => x.fk_ticket,
                        principalTable: "Tickets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<int>(type: "int", nullable: false),
                    fk_categoria = table.Column<int>(type: "int", nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.id);
                    table.ForeignKey(
                        name: "FK_Proveedor_Categorias_fk_categoria",
                        column: x => x.fk_categoria,
                        principalTable: "Categorias",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioTicket_fk_ticket",
                table: "ComentarioTicket",
                column: "fk_ticket");

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioTicket_fk_usuario",
                table: "ComentarioTicket",
                column: "fk_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_fk_categoria",
                table: "Proveedor",
                column: "fk_categoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComentarioTicket");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropColumn(
                name: "TiempoHastaCierre",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "cerrado_por_usuario",
                table: "Tickets");
        }
    }
}

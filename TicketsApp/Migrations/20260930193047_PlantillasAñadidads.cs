using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class PlantillasAñadidads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plantillas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    asunto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fk_categoria = table.Column<int>(type: "int", nullable: true),
                    solucion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    registro_propio_razon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_cierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_tomado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    asignado_a_provedor = table.Column<bool>(type: "bit", nullable: false),
                    fk_provedor = table.Column<int>(type: "int", nullable: true),
                    fk_usuario = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantillas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Plantillas_Categorias",
                        column: x => x.fk_categoria,
                        principalTable: "Categorias",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Plantillas_Provedores",
                        column: x => x.fk_provedor,
                        principalTable: "Proveedor",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Plantillas_Usuarios",
                        column: x => x.fk_usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plantillas_fk_categoria",
                table: "Plantillas",
                column: "fk_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Plantillas_fk_provedor",
                table: "Plantillas",
                column: "fk_provedor");

            migrationBuilder.CreateIndex(
                name: "IX_Plantillas_fk_usuario",
                table: "Plantillas",
                column: "fk_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plantillas");
        }
    }
}

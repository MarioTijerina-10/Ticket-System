using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class DepartamentosAñadidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proveedor_Categorias_fk_categoria",
                table: "Proveedor");

            migrationBuilder.AddColumn<int>(
                name: "FkDepartamento",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FkDepartamento",
                table: "AspNetUsers",
                column: "FkDepartamento");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Departamentos_FkDepartamento",
                table: "AspNetUsers",
                column: "FkDepartamento",
                principalTable: "Departamentos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedor_Categorias_fk_categoria",
                table: "Proveedor",
                column: "fk_categoria",
                principalTable: "Categorias",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departamentos_FkDepartamento",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Proveedor_Categorias_fk_categoria",
                table: "Proveedor");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FkDepartamento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FkDepartamento",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedor_Categorias_fk_categoria",
                table: "Proveedor",
                column: "fk_categoria",
                principalTable: "Categorias",
                principalColumn: "id");
        }
    }
}

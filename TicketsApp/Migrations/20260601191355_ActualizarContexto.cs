using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarContexto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioTicket_AspNetUsers_fk_usuario",
                table: "ComentarioTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioTicket_Tickets_fk_ticket",
                table: "ComentarioTicket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComentarioTicket",
                table: "ComentarioTicket");

            migrationBuilder.RenameTable(
                name: "ComentarioTicket",
                newName: "Comentarios");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioTicket_fk_usuario",
                table: "Comentarios",
                newName: "IX_Comentarios_fk_usuario");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioTicket_fk_ticket",
                table: "Comentarios",
                newName: "IX_Comentarios_fk_ticket");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Comentarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comentarios",
                table: "Comentarios",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_AspNetUsers_fk_usuario",
                table: "Comentarios",
                column: "fk_usuario",
                principalTable: "AspNetUsers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Tickets_fk_ticket",
                table: "Comentarios",
                column: "fk_ticket",
                principalTable: "Tickets",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_AspNetUsers_fk_usuario",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Tickets_fk_ticket",
                table: "Comentarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comentarios",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Comentarios");

            migrationBuilder.RenameTable(
                name: "Comentarios",
                newName: "ComentarioTicket");

            migrationBuilder.RenameIndex(
                name: "IX_Comentarios_fk_usuario",
                table: "ComentarioTicket",
                newName: "IX_ComentarioTicket_fk_usuario");

            migrationBuilder.RenameIndex(
                name: "IX_Comentarios_fk_ticket",
                table: "ComentarioTicket",
                newName: "IX_ComentarioTicket_fk_ticket");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComentarioTicket",
                table: "ComentarioTicket",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioTicket_AspNetUsers_fk_usuario",
                table: "ComentarioTicket",
                column: "fk_usuario",
                principalTable: "AspNetUsers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioTicket_Tickets_fk_ticket",
                table: "ComentarioTicket",
                column: "fk_ticket",
                principalTable: "Tickets",
                principalColumn: "id");
        }
    }
}

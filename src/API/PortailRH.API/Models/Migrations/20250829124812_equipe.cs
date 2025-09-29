using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortailRH.API.Migrations
{
    /// <inheritdoc />
    public partial class equipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Etablissement",
                table: "Contrats");

            migrationBuilder.DropColumn(
                name: "Fonction",
                table: "Contrats");

            migrationBuilder.DropColumn(
                name: "Salaire",
                table: "Contrats");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipeId",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Equipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EquipeId",
                table: "Employee",
                column: "EquipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Equipes_EquipeId",
                table: "Employee",
                column: "EquipeId",
                principalTable: "Equipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Equipes_EquipeId",
                table: "Employee");

            migrationBuilder.DropTable(
                name: "Equipes");

            migrationBuilder.DropIndex(
                name: "IX_Employee_EquipeId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EquipeId",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "Etablissement",
                table: "Contrats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fonction",
                table: "Contrats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Salaire",
                table: "Contrats",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

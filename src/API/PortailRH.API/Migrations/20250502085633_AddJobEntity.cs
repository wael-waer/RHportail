using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortailRH.API.Migrations
{
    /// <inheritdoc />
    public partial class AddJobEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Candidatures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_JobId",
                table: "Candidatures",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatures_Jobs_JobId",
                table: "Candidatures",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatures_Jobs_JobId",
                table: "Candidatures");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Candidatures_JobId",
                table: "Candidatures");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Candidatures");
        }
    }
}

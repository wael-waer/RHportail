using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortailRH.API.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatures_Jobs_JobId",
                table: "Candidatures");

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Candidatures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatures_Jobs_JobId",
                table: "Candidatures",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatures_Jobs_JobId",
                table: "Candidatures");

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Candidatures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatures_Jobs_JobId",
                table: "Candidatures",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}

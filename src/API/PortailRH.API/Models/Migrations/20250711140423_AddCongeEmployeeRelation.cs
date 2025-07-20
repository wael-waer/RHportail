using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortailRH.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCongeEmployeeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdEmploye",
                table: "Conges",
                newName: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conges_EmployeeId",
                table: "Conges",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conges_Employee_EmployeeId",
                table: "Conges",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conges_Employee_EmployeeId",
                table: "Conges");

            migrationBuilder.DropIndex(
                name: "IX_Conges_EmployeeId",
                table: "Conges");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Conges",
                newName: "IdEmploye");
        }
    }
}

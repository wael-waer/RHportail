using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortailRH.API.Migrations
{
    /// <inheritdoc />
    public partial class Addemploye : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contrats_Employee_EmployeeId",
                table: "contrats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contrats",
                table: "contrats");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SuiviConges");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PaymentPolicies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Conges");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Candidatures");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "contrats",
                newName: "Contrats");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Employee",
                newName: "Fonction");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Contrats",
                newName: "Typecontrat");

            migrationBuilder.RenameIndex(
                name: "IX_contrats_EmployeeId",
                table: "Contrats",
                newName: "IX_Contrats_EmployeeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEntree",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Etablissement",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Salaire",
                table: "Employee",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SoldeCongeMaladie",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contrats",
                table: "Contrats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contrats_Employee_EmployeeId",
                table: "Contrats",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrats_Employee_EmployeeId",
                table: "Contrats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contrats",
                table: "Contrats");

            migrationBuilder.DropColumn(
                name: "DateEntree",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Etablissement",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Salaire",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SoldeCongeMaladie",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Contrats",
                newName: "contrats");

            migrationBuilder.RenameColumn(
                name: "Fonction",
                table: "Employee",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Typecontrat",
                table: "contrats",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Contrats_EmployeeId",
                table: "contrats",
                newName: "IX_contrats_EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SuiviConges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Payslips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PaymentPolicies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Conges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Candidatures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contrats",
                table: "contrats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_contrats_Employee_EmployeeId",
                table: "contrats",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortailRH.API.Migrations
{
    /// <inheritdoc />
    public partial class Updateemployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "Employees",
                newName: "Salaire");

            migrationBuilder.RenameColumn(
                name: "Poste",
                table: "Employees",
                newName: "Prenom");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Employees",
                newName: "NumeroTelephone");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Employees",
                newName: "NumeroIdentification");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Employees",
                newName: "DateNaissance");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEntree",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmailSecondaire",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Etablissement",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fonction",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MotDePasse",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroTelephoneSecondaire",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoldeConge",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoldeCongeMaladie",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateEntree",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmailSecondaire",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Etablissement",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Fonction",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MotDePasse",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NumeroTelephoneSecondaire",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SoldeConge",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SoldeCongeMaladie",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Salaire",
                table: "Employees",
                newName: "Salary");

            migrationBuilder.RenameColumn(
                name: "Prenom",
                table: "Employees",
                newName: "Poste");

            migrationBuilder.RenameColumn(
                name: "NumeroTelephone",
                table: "Employees",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "NumeroIdentification",
                table: "Employees",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "DateNaissance",
                table: "Employees",
                newName: "BirthDate");
        }
    }
}

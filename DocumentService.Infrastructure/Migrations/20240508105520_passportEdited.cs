using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class passportEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "IssuedBy",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "PlaceOfBirth",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "SeriesAndNumber",
                table: "Passports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "Passports",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "IssuedBy",
                table: "Passports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlaceOfBirth",
                table: "Passports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeriesAndNumber",
                table: "Passports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

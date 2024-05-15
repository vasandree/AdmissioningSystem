using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class passportInfoAndEducationDocumentInfoAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Passports",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "Passports",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IssuedBy",
                table: "Passports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeriesAndNumber",
                table: "Passports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EducationDocumentTypeId",
                table: "EducationDocuments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EducationDocuments",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EducationDocumentTypeDto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EducationLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDocumentTypeDto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationLevelDto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    EducationDocumentTypeDtoId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationLevelDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationLevelDto_EducationDocumentTypeDto_EducationDocumen~",
                        column: x => x.EducationDocumentTypeDtoId,
                        principalTable: "EducationDocumentTypeDto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationDocuments_EducationDocumentTypeId",
                table: "EducationDocuments",
                column: "EducationDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationDocumentTypeDto_EducationLevelId",
                table: "EducationDocumentTypeDto",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevelDto_EducationDocumentTypeDtoId",
                table: "EducationLevelDto",
                column: "EducationDocumentTypeDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationDocuments_EducationDocumentTypeDto_EducationDocume~",
                table: "EducationDocuments",
                column: "EducationDocumentTypeId",
                principalTable: "EducationDocumentTypeDto",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationDocumentTypeDto_EducationLevelDto_EducationLevelId",
                table: "EducationDocumentTypeDto",
                column: "EducationLevelId",
                principalTable: "EducationLevelDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationDocuments_EducationDocumentTypeDto_EducationDocume~",
                table: "EducationDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationDocumentTypeDto_EducationLevelDto_EducationLevelId",
                table: "EducationDocumentTypeDto");

            migrationBuilder.DropTable(
                name: "EducationLevelDto");

            migrationBuilder.DropTable(
                name: "EducationDocumentTypeDto");

            migrationBuilder.DropIndex(
                name: "IX_EducationDocuments_EducationDocumentTypeId",
                table: "EducationDocuments");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "IssuedBy",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "SeriesAndNumber",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "EducationDocumentTypeId",
                table: "EducationDocuments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EducationDocuments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictionaryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nextEducationLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NextEducationLevels_DocumentTypes_DocumentTypeId",
                table: "NextEducationLevels");

            migrationBuilder.DropIndex(
                name: "IX_NextEducationLevels_DocumentTypeId",
                table: "NextEducationLevels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_NextEducationLevels_DocumentTypeId",
                table: "NextEducationLevels",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NextEducationLevels_DocumentTypes_DocumentTypeId",
                table: "NextEducationLevels",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

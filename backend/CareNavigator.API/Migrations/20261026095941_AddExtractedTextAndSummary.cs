using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareNavigator.API.Migrations
{
    /// <inheritdoc />
    public partial class AddExtractedTextAndSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtractedText",
                table: "MedicalReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "MedicalReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtractedText",
                table: "MedicalReports");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "MedicalReports");
        }
    }
}

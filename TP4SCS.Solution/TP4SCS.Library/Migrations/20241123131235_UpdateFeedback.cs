using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP4SCS.Library.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsParentFeedback",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "ParentFeedbackId",
                table: "Feedback");

            migrationBuilder.AddColumn<string>(
                name: "Reply",
                table: "Feedback",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reply",
                table: "Feedback");

            migrationBuilder.AddColumn<bool>(
                name: "IsParentFeedback",
                table: "Feedback",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentFeedbackId",
                table: "Feedback",
                type: "int",
                nullable: true);
        }
    }
}

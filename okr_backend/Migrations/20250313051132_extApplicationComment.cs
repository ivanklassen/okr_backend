using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace okr_backend.Migrations
{
    /// <inheritdoc />
    public partial class extApplicationComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "extensionApplications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comment",
                table: "extensionApplications");
        }
    }
}

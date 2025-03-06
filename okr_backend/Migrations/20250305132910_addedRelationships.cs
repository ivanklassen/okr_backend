using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace okr_backend.Migrations
{
    /// <inheritdoc />
    public partial class addedRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_extensionApplications_applicationId",
                table: "extensionApplications",
                column: "applicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_userId",
                table: "Applications",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_userId",
                table: "Applications",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_extensionApplications_Applications_applicationId",
                table: "extensionApplications",
                column: "applicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_userId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_extensionApplications_Applications_applicationId",
                table: "extensionApplications");

            migrationBuilder.DropIndex(
                name: "IX_extensionApplications_applicationId",
                table: "extensionApplications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_userId",
                table: "Applications");
        }
    }
}

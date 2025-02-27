using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace okr_backend.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullName = table.Column<string>(type: "text", nullable: false),
                    birthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    confirmPassword = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    isStudent = table.Column<bool>(type: "boolean", nullable: false),
                    isTeacher = table.Column<bool>(type: "boolean", nullable: false),
                    isDean = table.Column<bool>(type: "boolean", nullable: false),
                    isAdmin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

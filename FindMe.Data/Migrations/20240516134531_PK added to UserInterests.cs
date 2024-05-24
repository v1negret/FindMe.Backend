using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindMe.Data.Migrations
{
    /// <inheritdoc />
    public partial class PKaddedtoUserInterests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UsersInterests",
                newName: "UserInterest");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest",
                columns: new[] { "UserId", "InterestId" });

            migrationBuilder.CreateTable(
                name: "ViewedForms",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    FormId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewedForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest");

            migrationBuilder.RenameTable(
                name: "UserInterest",
                newName: "UsersInterests");
        }
    }
}

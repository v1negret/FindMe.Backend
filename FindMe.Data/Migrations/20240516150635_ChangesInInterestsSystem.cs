using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindMe.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInInterestsSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest");

            migrationBuilder.RenameTable(
                name: "UserInterest",
                newName: "UserInterests");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserInterests",
                newName: "FormId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInterests",
                table: "UserInterests",
                columns: new[] { "FormId", "InterestId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInterests",
                table: "UserInterests");

            migrationBuilder.RenameTable(
                name: "UserInterests",
                newName: "UserInterest");

            migrationBuilder.RenameColumn(
                name: "FormId",
                table: "UserInterest",
                newName: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest",
                columns: new[] { "UserId", "InterestId" });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindMe.Data.Migrations
{
    /// <inheritdoc />
    public partial class SomeChangesEverywhere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ViewedForms",
                newName: "ViewedFormId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ViewedForms",
                table: "ViewedForms",
                columns: new[] { "FormId", "ViewedFormId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterests_Forms_FormId",
                table: "UserInterests",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInterests_Forms_FormId",
                table: "UserInterests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ViewedForms",
                table: "ViewedForms");

            migrationBuilder.RenameColumn(
                name: "ViewedFormId",
                table: "ViewedForms",
                newName: "UserId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Test5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Languages_LanguageId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "Posts",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_LanguageId",
                table: "Posts",
                newName: "IX_Posts_CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Categories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowOnHomePage",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_LanguageId",
                table: "Categories",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Languages_LanguageId",
                table: "Categories",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Categories_CategoryId",
                table: "Posts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Languages_LanguageId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Categories_CategoryId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Categories_LanguageId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ShowOnHomePage",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Posts",
                newName: "LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                newName: "IX_Posts_LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Languages_LanguageId",
                table: "Posts",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

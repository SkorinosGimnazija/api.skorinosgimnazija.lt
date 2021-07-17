using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Test4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Languages",
                newName: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Slug",
                table: "Languages",
                column: "Slug",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Languages_Slug",
                table: "Languages");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Languages",
                newName: "Code");
        }
    }
}

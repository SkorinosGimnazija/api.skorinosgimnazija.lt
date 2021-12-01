using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Menu1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Menus_Slug",
                table: "Menus");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Slug_LanguageId",
                table: "Menus",
                columns: new[] { "Slug", "LanguageId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Menus_Slug_LanguageId",
                table: "Menus");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Slug",
                table: "Menus",
                column: "Slug",
                unique: true);
        }
    }
}

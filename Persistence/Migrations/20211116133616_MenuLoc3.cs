using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class MenuLoc3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_MenuLocations_MenuLocationId",
                table: "Menus");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_MenuLocations_MenuLocationId",
                table: "Menus",
                column: "MenuLocationId",
                principalTable: "MenuLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_MenuLocations_MenuLocationId",
                table: "Menus");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_MenuLocations_MenuLocationId",
                table: "Menus",
                column: "MenuLocationId",
                principalTable: "MenuLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

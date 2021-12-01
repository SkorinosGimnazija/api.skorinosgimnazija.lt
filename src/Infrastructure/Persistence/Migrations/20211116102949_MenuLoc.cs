using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class MenuLoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuLocationId",
                table: "Menus",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MenuLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuLocations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MenuLocationId",
                table: "Menus",
                column: "MenuLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuLocations_Slug",
                table: "MenuLocations",
                column: "Slug",
                unique: true);

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

            migrationBuilder.DropTable(
                name: "MenuLocations");

            migrationBuilder.DropIndex(
                name: "IX_Menus_MenuLocationId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "MenuLocationId",
                table: "Menus");
        }
    }
}

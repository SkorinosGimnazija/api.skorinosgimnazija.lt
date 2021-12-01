using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class Test12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Menus");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Menus",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Menus",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LinkedPostId",
                table: "Menus",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Menus",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_LinkedPostId",
                table: "Menus",
                column: "LinkedPostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Posts_LinkedPostId",
                table: "Menus",
                column: "LinkedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Posts_LinkedPostId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_LinkedPostId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "LinkedPostId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Menus");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Menus",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Menus",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Menus",
                type: "text",
                nullable: true);
        }
    }
}

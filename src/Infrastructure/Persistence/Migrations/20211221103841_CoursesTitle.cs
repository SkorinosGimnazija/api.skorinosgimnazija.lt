using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class CoursesTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Courses",
                newName: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Courses",
                newName: "Name");
        }
    }
}

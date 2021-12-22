using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class CoursesFloatDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "DurationInHours",
                table: "Courses",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DurationInHours",
                table: "Courses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}

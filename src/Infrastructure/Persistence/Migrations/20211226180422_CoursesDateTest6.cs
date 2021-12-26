using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class CoursesDateTest6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_StartDate",
                table: "Courses");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Courses",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StartDate",
                table: "Courses",
                column: "StartDate");
        }
    }
}

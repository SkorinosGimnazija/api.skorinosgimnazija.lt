using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class CoursesDateTest5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "Posts",
                newName: "PublishedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "Posts",
                newName: "ModifiedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Courses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                table: "Posts",
                newName: "PublishDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Posts",
                newName: "ModifiedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "Courses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

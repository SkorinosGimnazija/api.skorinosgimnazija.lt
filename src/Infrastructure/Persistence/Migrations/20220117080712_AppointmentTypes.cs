using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class AppointmentTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "AppointmentTypes",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "AppointmentTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationEnd",
                table: "AppointmentTypes",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "AppointmentTypes",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes",
                column: "Slug",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes");

            migrationBuilder.DropColumn(
                name: "End",
                table: "AppointmentTypes");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "AppointmentTypes");

            migrationBuilder.DropColumn(
                name: "RegistrationEnd",
                table: "AppointmentTypes");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "AppointmentTypes");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes",
                column: "Slug");
        }
    }
}

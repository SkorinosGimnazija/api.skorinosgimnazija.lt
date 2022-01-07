using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class Appointments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "AppointmentTypes");

            migrationBuilder.AddColumn<int>(
                name: "DurationInMinutes",
                table: "AppointmentTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInMinutes",
                table: "AppointmentTypes");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "AppointmentTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

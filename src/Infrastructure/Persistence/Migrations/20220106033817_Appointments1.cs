using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class Appointments1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentTypes_TypeId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_TypeId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "AppointmentDates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDates_TypeId",
                table: "AppointmentDates",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDates_AppointmentTypes_TypeId",
                table: "AppointmentDates",
                column: "TypeId",
                principalTable: "AppointmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDates_AppointmentTypes_TypeId",
                table: "AppointmentDates");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDates_TypeId",
                table: "AppointmentDates");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "AppointmentDates");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TypeId",
                table: "Appointments",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentTypes_TypeId",
                table: "Appointments",
                column: "TypeId",
                principalTable: "AppointmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

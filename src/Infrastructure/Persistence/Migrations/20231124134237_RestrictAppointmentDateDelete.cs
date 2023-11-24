using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RestrictAppointmentDateDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentDates_DateId",
                table: "Appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentDates_DateId",
                table: "Appointments",
                column: "DateId",
                principalTable: "AppointmentDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentDates_DateId",
                table: "Appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentDates_DateId",
                table: "Appointments",
                column: "DateId",
                principalTable: "AppointmentDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

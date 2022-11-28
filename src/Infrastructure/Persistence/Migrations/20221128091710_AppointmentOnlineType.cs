using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class AppointmentOnlineType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDates_AppointmentTypes_TypeId",
                table: "AppointmentDates");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentDates_DateId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "AppointmentTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "EventMeetingLink",
                table: "Appointments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes",
                column: "Slug");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDates_AppointmentTypes_TypeId",
                table: "AppointmentDates",
                column: "TypeId",
                principalTable: "AppointmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentDates_DateId",
                table: "Appointments",
                column: "DateId",
                principalTable: "AppointmentDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDates_AppointmentTypes_TypeId",
                table: "AppointmentDates");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentDates_DateId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "AppointmentTypes");

            migrationBuilder.AlterColumn<string>(
                name: "EventMeetingLink",
                table: "Appointments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentTypes_Slug",
                table: "AppointmentTypes",
                column: "Slug",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDates_AppointmentTypes_TypeId",
                table: "AppointmentDates",
                column: "TypeId",
                principalTable: "AppointmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentDates_DateId",
                table: "Appointments",
                column: "DateId",
                principalTable: "AppointmentDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

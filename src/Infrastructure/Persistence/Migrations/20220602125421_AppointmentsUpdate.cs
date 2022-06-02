using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class AppointmentsUpdate : Migration
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
                name: "IX_Appointments_AttendeeEmail",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "End",
                table: "AppointmentTypes");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "AppointmentTypes");

            migrationBuilder.AddColumn<string>(
                name: "AttendeeUserName",
                table: "Appointments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventMeetingLink",
                table: "Appointments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserDisplayName",
                table: "Appointments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppointmentExclusiveHosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentExclusiveHosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentExclusiveHosts_AppointmentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "AppointmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AttendeeUserName",
                table: "Appointments",
                column: "AttendeeUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserName_AttendeeEmail",
                table: "Appointments",
                columns: new[] { "UserName", "AttendeeEmail" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentReservedDates_UserName_DateId",
                table: "AppointmentReservedDates",
                columns: new[] { "UserName", "DateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDates_Date",
                table: "AppointmentDates",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentExclusiveHosts_TypeId",
                table: "AppointmentExclusiveHosts",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentExclusiveHosts_UserName",
                table: "AppointmentExclusiveHosts",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentExclusiveHosts_UserName_TypeId",
                table: "AppointmentExclusiveHosts",
                columns: new[] { "UserName", "TypeId" },
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDates_AppointmentTypes_TypeId",
                table: "AppointmentDates");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentDates_DateId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "AppointmentExclusiveHosts");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AttendeeUserName",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_UserName_AttendeeEmail",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentReservedDates_UserName_DateId",
                table: "AppointmentReservedDates");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDates_Date",
                table: "AppointmentDates");

            migrationBuilder.DropColumn(
                name: "AttendeeUserName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EventMeetingLink",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "UserDisplayName",
                table: "Appointments");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
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
                name: "IX_Appointments_AttendeeEmail",
                table: "Appointments",
                column: "AttendeeEmail");

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
    }
}

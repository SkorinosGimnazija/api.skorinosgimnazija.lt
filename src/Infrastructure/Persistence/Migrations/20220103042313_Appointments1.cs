using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class Appointments1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReporterInfo",
                table: "BullyReports");

            migrationBuilder.CreateTable(
                name: "ParentAppointmentDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentAppointmentDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParentAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<string>(type: "text", nullable: false),
                    DateId = table.Column<int>(type: "integer", nullable: false),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    ParentName = table.Column<string>(type: "text", nullable: false),
                    ParentEmail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentAppointments_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentAppointments_ParentAppointmentDates_DateId",
                        column: x => x.DateId,
                        principalTable: "ParentAppointmentDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentAppointments_DateId",
                table: "ParentAppointments",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentAppointments_TeacherId",
                table: "ParentAppointments",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentAppointments");

            migrationBuilder.DropTable(
                name: "ParentAppointmentDates");

            migrationBuilder.AddColumn<string>(
                name: "ReporterInfo",
                table: "BullyReports",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ClasstimeShortDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTimeShort",
                table: "Classtimes",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTimeShort",
                table: "Classtimes",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateTable(
                name: "ClasstimeShortDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClasstimeShortDays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_StartTime",
                table: "Announcements",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_ClasstimeShortDays_Date",
                table: "ClasstimeShortDays",
                column: "Date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClasstimeShortDays");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_StartTime",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "EndTimeShort",
                table: "Classtimes");

            migrationBuilder.DropColumn(
                name: "StartTimeShort",
                table: "Classtimes");
        }
    }
}

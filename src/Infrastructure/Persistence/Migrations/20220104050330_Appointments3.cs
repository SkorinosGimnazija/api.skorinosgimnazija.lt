using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class Appointments3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentAppointments_AspNetUsers_TeacherId",
                table: "ParentAppointments");

            migrationBuilder.DropIndex(
                name: "IX_ParentAppointments_TeacherId",
                table: "ParentAppointments");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "ParentAppointments",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "ParentName",
                table: "ParentAppointments",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ParentEmail",
                table: "ParentAppointments",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "ParentAppointments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ParentAppointments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ParentAppointmentReservedDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentAppointmentReservedDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentAppointmentReservedDates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentAppointmentReservedDates_ParentAppointmentDates_DateId",
                        column: x => x.DateId,
                        principalTable: "ParentAppointmentDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentAppointments_ParentEmail_DateId",
                table: "ParentAppointments",
                columns: new[] { "ParentEmail", "DateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParentAppointments_UserId_DateId",
                table: "ParentAppointments",
                columns: new[] { "UserId", "DateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParentAppointments_UserName",
                table: "ParentAppointments",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_ParentAppointmentReservedDates_DateId",
                table: "ParentAppointmentReservedDates",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentAppointmentReservedDates_UserId",
                table: "ParentAppointmentReservedDates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentAppointmentReservedDates_UserName",
                table: "ParentAppointmentReservedDates",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentAppointments_AspNetUsers_UserId",
                table: "ParentAppointments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentAppointments_AspNetUsers_UserId",
                table: "ParentAppointments");

            migrationBuilder.DropTable(
                name: "ParentAppointmentReservedDates");

            migrationBuilder.DropIndex(
                name: "IX_ParentAppointments_ParentEmail_DateId",
                table: "ParentAppointments");

            migrationBuilder.DropIndex(
                name: "IX_ParentAppointments_UserId_DateId",
                table: "ParentAppointments");

            migrationBuilder.DropIndex(
                name: "IX_ParentAppointments_UserName",
                table: "ParentAppointments");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ParentAppointments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ParentAppointments",
                newName: "TeacherId");

            migrationBuilder.AlterColumn<string>(
                name: "ParentName",
                table: "ParentAppointments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "ParentEmail",
                table: "ParentAppointments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "ParentAppointments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_ParentAppointments_TeacherId",
                table: "ParentAppointments",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentAppointments_AspNetUsers_TeacherId",
                table: "ParentAppointments",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

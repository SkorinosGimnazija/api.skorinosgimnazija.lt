using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Timetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classtimes_Number",
                table: "Classtimes");

            migrationBuilder.DropIndex(
                name: "IX_AccomplishmentClassrooms_Number",
                table: "AccomplishmentClassrooms");

            migrationBuilder.CreateTable(
                name: "Classdays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classdays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DayId = table.Column<int>(type: "integer", nullable: false),
                    TimeId = table.Column<int>(type: "integer", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    ClassName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timetable_AccomplishmentClassrooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "AccomplishmentClassrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetable_Classdays_DayId",
                        column: x => x.DayId,
                        principalTable: "Classdays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetable_Classtimes_TimeId",
                        column: x => x.TimeId,
                        principalTable: "Classtimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classtimes_Number",
                table: "Classtimes",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccomplishmentClassrooms_Number",
                table: "AccomplishmentClassrooms",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classdays_Number",
                table: "Classdays",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetable_DayId_RoomId_TimeId",
                table: "Timetable",
                columns: new[] { "DayId", "RoomId", "TimeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetable_RoomId",
                table: "Timetable",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetable_TimeId",
                table: "Timetable",
                column: "TimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timetable");

            migrationBuilder.DropTable(
                name: "Classdays");

            migrationBuilder.DropIndex(
                name: "IX_Classtimes_Number",
                table: "Classtimes");

            migrationBuilder.DropIndex(
                name: "IX_AccomplishmentClassrooms_Number",
                table: "AccomplishmentClassrooms");

            migrationBuilder.CreateIndex(
                name: "IX_Classtimes_Number",
                table: "Classtimes",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_AccomplishmentClassrooms_Number",
                table: "AccomplishmentClassrooms",
                column: "Number");
        }
    }
}

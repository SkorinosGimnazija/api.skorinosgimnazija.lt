using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StudentObservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObservationLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationLessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObservationTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObservationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentObservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TargetId = table.Column<int>(type: "integer", nullable: false),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentObservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentObservations_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentObservations_ObservationLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "ObservationLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentObservations_ObservationTargets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "ObservationTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObservationTypeStudentObservation",
                columns: table => new
                {
                    StudentObservationsId = table.Column<int>(type: "integer", nullable: false),
                    TypesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationTypeStudentObservation", x => new { x.StudentObservationsId, x.TypesId });
                    table.ForeignKey(
                        name: "FK_ObservationTypeStudentObservation_ObservationTypes_TypesId",
                        column: x => x.TypesId,
                        principalTable: "ObservationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ObservationTypeStudentObservation_StudentObservations_Stude~",
                        column: x => x.StudentObservationsId,
                        principalTable: "StudentObservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObservationTypeStudentObservation_TypesId",
                table: "ObservationTypeStudentObservation",
                column: "TypesId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentObservations_LessonId",
                table: "StudentObservations",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentObservations_TargetId",
                table: "StudentObservations",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentObservations_TeacherId",
                table: "StudentObservations",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObservationTypeStudentObservation");

            migrationBuilder.DropTable(
                name: "ObservationTypes");

            migrationBuilder.DropTable(
                name: "StudentObservations");

            migrationBuilder.DropTable(
                name: "ObservationLessons");

            migrationBuilder.DropTable(
                name: "ObservationTargets");
        }
    }
}

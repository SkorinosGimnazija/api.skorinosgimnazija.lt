using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class Accomplishments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccomplishmentClassrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomplishmentClassrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccomplishmentScales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomplishmentScales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accomplishments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Achievement = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ScaleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accomplishments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accomplishments_AccomplishmentScales_ScaleId",
                        column: x => x.ScaleId,
                        principalTable: "AccomplishmentScales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accomplishments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccomplishmentAdditionalTeachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    AccomplishmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomplishmentAdditionalTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccomplishmentAdditionalTeachers_Accomplishments_Accomplish~",
                        column: x => x.AccomplishmentId,
                        principalTable: "Accomplishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccomplishmentStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    AccomplishmentId = table.Column<int>(type: "integer", nullable: false),
                    ClassroomId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomplishmentStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccomplishmentStudents_AccomplishmentClassrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "AccomplishmentClassrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccomplishmentStudents_Accomplishments_AccomplishmentId",
                        column: x => x.AccomplishmentId,
                        principalTable: "Accomplishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccomplishmentAdditionalTeachers_AccomplishmentId",
                table: "AccomplishmentAdditionalTeachers",
                column: "AccomplishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Accomplishments_Date",
                table: "Accomplishments",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Accomplishments_ScaleId",
                table: "Accomplishments",
                column: "ScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Accomplishments_UserId",
                table: "Accomplishments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccomplishmentStudents_AccomplishmentId",
                table: "AccomplishmentStudents",
                column: "AccomplishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AccomplishmentStudents_ClassroomId",
                table: "AccomplishmentStudents",
                column: "ClassroomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccomplishmentAdditionalTeachers");

            migrationBuilder.DropTable(
                name: "AccomplishmentStudents");

            migrationBuilder.DropTable(
                name: "AccomplishmentClassrooms");

            migrationBuilder.DropTable(
                name: "Accomplishments");

            migrationBuilder.DropTable(
                name: "AccomplishmentScales");
        }
    }
}

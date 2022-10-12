using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class BullyJournal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BullyJournalReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BullyInfo = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    VictimInfo = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Details = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Actions = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BullyJournalReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BullyJournalReports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BullyJournalReports_Date",
                table: "BullyJournalReports",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_BullyJournalReports_UserId",
                table: "BullyJournalReports",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BullyJournalReports");
        }
    }
}

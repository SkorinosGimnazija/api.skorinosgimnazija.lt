using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    public partial class AccomplishmentAchievements2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Achievement",
                table: "Accomplishments");

            migrationBuilder.AddColumn<int>(
                name: "AchievementId",
                table: "AccomplishmentStudents",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_AccomplishmentStudents_AchievementId",
                table: "AccomplishmentStudents",
                column: "AchievementId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccomplishmentStudents_AccomplishmentAchievements_Achieveme~",
                table: "AccomplishmentStudents",
                column: "AchievementId",
                principalTable: "AccomplishmentAchievements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccomplishmentStudents_AccomplishmentAchievements_Achieveme~",
                table: "AccomplishmentStudents");

            migrationBuilder.DropIndex(
                name: "IX_AccomplishmentStudents_AchievementId",
                table: "AccomplishmentStudents");

            migrationBuilder.DropColumn(
                name: "AchievementId",
                table: "AccomplishmentStudents");

            migrationBuilder.AddColumn<string>(
                name: "Achievement",
                table: "Accomplishments",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }
    }
}

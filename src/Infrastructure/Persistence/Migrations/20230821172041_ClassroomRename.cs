using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkorinosGimnazija.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ClassroomRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "AccomplishmentClassrooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AccomplishmentClassrooms_Number",
                table: "AccomplishmentClassrooms",
                column: "Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccomplishmentClassrooms_Number",
                table: "AccomplishmentClassrooms");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "AccomplishmentClassrooms");
        }
    }
}

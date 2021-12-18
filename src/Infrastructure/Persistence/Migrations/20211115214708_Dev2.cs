﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Dev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Languages_LanguageId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_LanguageId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Menus_Slug_LanguageId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Slug",
                table: "Menus",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_LanguageId",
                table: "Categories",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Languages_LanguageId",
                table: "Categories",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Languages_LanguageId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Menus_Slug",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Categories_LanguageId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LanguageId",
                table: "Posts",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Slug_LanguageId",
                table: "Menus",
                columns: new[] { "Slug", "LanguageId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Languages_LanguageId",
                table: "Posts",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
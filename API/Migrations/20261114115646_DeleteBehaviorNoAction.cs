using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBehaviorNoAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_AchievementScales_ScaleId",
                table: "Achievements");

            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Users_CreatorId",
                table: "Achievements");

            migrationBuilder.DropForeignKey(
                name: "FK_AchievementStudents_Classrooms_ClassroomId",
                table: "AchievementStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentDates_AppointmentDateId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Languages_LanguageId",
                table: "Banners");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Languages_LanguageId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationObservationOption_ObservationOptions_OptionsId",
                table: "ObservationObservationOption");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_ObservationLessons_LessonId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_ObservationStudents_StudentId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Users_CreatorId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Languages_LanguageId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Classdays_DayId",
                table: "Timetable");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Classrooms_RoomId",
                table: "Timetable");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Classtimes_TimeId",
                table: "Timetable");

            migrationBuilder.DropForeignKey(
                name: "FK_TimetableOverrides_Classrooms_RoomId",
                table: "TimetableOverrides");

            migrationBuilder.DropForeignKey(
                name: "FK_TimetableOverrides_Classtimes_TimeId",
                table: "TimetableOverrides");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_AchievementScales_ScaleId",
                table: "Achievements",
                column: "ScaleId",
                principalTable: "AchievementScales",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Users_CreatorId",
                table: "Achievements",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AchievementStudents_Classrooms_ClassroomId",
                table: "AchievementStudents",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentDates_AppointmentDateId",
                table: "Appointments",
                column: "AppointmentDateId",
                principalTable: "AppointmentDates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Languages_LanguageId",
                table: "Banners",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Languages_LanguageId",
                table: "Menus",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationObservationOption_ObservationOptions_OptionsId",
                table: "ObservationObservationOption",
                column: "OptionsId",
                principalTable: "ObservationOptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_ObservationLessons_LessonId",
                table: "Observations",
                column: "LessonId",
                principalTable: "ObservationLessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_ObservationStudents_StudentId",
                table: "Observations",
                column: "StudentId",
                principalTable: "ObservationStudents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Users_CreatorId",
                table: "Observations",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Languages_LanguageId",
                table: "Posts",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Classdays_DayId",
                table: "Timetable",
                column: "DayId",
                principalTable: "Classdays",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Classrooms_RoomId",
                table: "Timetable",
                column: "RoomId",
                principalTable: "Classrooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Classtimes_TimeId",
                table: "Timetable",
                column: "TimeId",
                principalTable: "Classtimes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimetableOverrides_Classrooms_RoomId",
                table: "TimetableOverrides",
                column: "RoomId",
                principalTable: "Classrooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimetableOverrides_Classtimes_TimeId",
                table: "TimetableOverrides",
                column: "TimeId",
                principalTable: "Classtimes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_AchievementScales_ScaleId",
                table: "Achievements");

            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Users_CreatorId",
                table: "Achievements");

            migrationBuilder.DropForeignKey(
                name: "FK_AchievementStudents_Classrooms_ClassroomId",
                table: "AchievementStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentDates_AppointmentDateId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Languages_LanguageId",
                table: "Banners");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Languages_LanguageId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationObservationOption_ObservationOptions_OptionsId",
                table: "ObservationObservationOption");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_ObservationLessons_LessonId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_ObservationStudents_StudentId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Users_CreatorId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Languages_LanguageId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Classdays_DayId",
                table: "Timetable");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Classrooms_RoomId",
                table: "Timetable");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Classtimes_TimeId",
                table: "Timetable");

            migrationBuilder.DropForeignKey(
                name: "FK_TimetableOverrides_Classrooms_RoomId",
                table: "TimetableOverrides");

            migrationBuilder.DropForeignKey(
                name: "FK_TimetableOverrides_Classtimes_TimeId",
                table: "TimetableOverrides");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_AchievementScales_ScaleId",
                table: "Achievements",
                column: "ScaleId",
                principalTable: "AchievementScales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Users_CreatorId",
                table: "Achievements",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AchievementStudents_Classrooms_ClassroomId",
                table: "AchievementStudents",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentDates_AppointmentDateId",
                table: "Appointments",
                column: "AppointmentDateId",
                principalTable: "AppointmentDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Languages_LanguageId",
                table: "Banners",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Languages_LanguageId",
                table: "Menus",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationObservationOption_ObservationOptions_OptionsId",
                table: "ObservationObservationOption",
                column: "OptionsId",
                principalTable: "ObservationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_ObservationLessons_LessonId",
                table: "Observations",
                column: "LessonId",
                principalTable: "ObservationLessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_ObservationStudents_StudentId",
                table: "Observations",
                column: "StudentId",
                principalTable: "ObservationStudents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Users_CreatorId",
                table: "Observations",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Languages_LanguageId",
                table: "Posts",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Classdays_DayId",
                table: "Timetable",
                column: "DayId",
                principalTable: "Classdays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Classrooms_RoomId",
                table: "Timetable",
                column: "RoomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Classtimes_TimeId",
                table: "Timetable",
                column: "TimeId",
                principalTable: "Classtimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimetableOverrides_Classrooms_RoomId",
                table: "TimetableOverrides",
                column: "RoomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimetableOverrides_Classtimes_TimeId",
                table: "TimetableOverrides",
                column: "TimeId",
                principalTable: "Classtimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

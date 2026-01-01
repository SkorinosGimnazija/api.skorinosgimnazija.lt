using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

            migrationBuilder.CreateTable(
                name: "AchievementScales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementScales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AchievementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DurationInMinutes = table.Column<int>(type: "integer", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false),
                    RegistrationEndsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classdays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classdays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classtimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    StartTimeShort = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTimeShort = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classtimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobRecord",
                columns: table => new
                {
                    TrackingID = table.Column<Guid>(type: "uuid", nullable: false),
                    QueueID = table.Column<string>(type: "text", nullable: false),
                    Command = table.Column<string>(type: "jsonb", nullable: false),
                    ExecuteAfter = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpireOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRecord", x => x.TrackingID);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObservationLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationLessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObservationOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObservationStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationStudents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShortDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false, computedColumnSql: "translate(upper(\"Name\"), 'ĄČĘĖĮŠŲŪŽ', 'ACEEISUUZ')", stored: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    IsSuspended = table.Column<bool>(type: "boolean", nullable: false),
                    IsTeacher = table.Column<bool>(type: "boolean", nullable: false),
                    Roles = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentDates_AppointmentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "AppointmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timetable",
                columns: table => new
                {
                    TimeId = table.Column<int>(type: "integer", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    DayId = table.Column<int>(type: "integer", nullable: false),
                    ClassName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetable", x => new { x.DayId, x.TimeId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_Timetable_Classdays_DayId",
                        column: x => x.DayId,
                        principalTable: "Classdays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetable_Classrooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetable_Classtimes_TimeId",
                        column: x => x.TimeId,
                        principalTable: "Classtimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimetableOverrides",
                columns: table => new
                {
                    TimeId = table.Column<int>(type: "integer", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ClassName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableOverrides", x => new { x.Date, x.TimeId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_TimetableOverrides_Classrooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimetableOverrides_Classtimes_TimeId",
                        column: x => x.TimeId,
                        principalTable: "Classtimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedTitle = table.Column<string>(type: "text", nullable: false, computedColumnSql: "translate(upper(\"Title\"), 'ĄČĘĖĮŠŲŪŽ', 'ACEEISUUZ')", stored: true),
                    Url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<string>(type: "character varying(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banners_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    ShowInFeed = table.Column<bool>(type: "boolean", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LanguageId = table.Column<string>(type: "character varying(5)", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedTitle = table.Column<string>(type: "text", nullable: false, computedColumnSql: "translate(upper(\"Title\"), 'ĄČĘĖĮŠŲŪŽ', 'ACEEISUUZ')", stored: true),
                    Slug = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IntroText = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    FeaturedImage = table.Column<string>(type: "text", nullable: true),
                    Meta = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Files = table.Column<List<string>>(type: "text[]", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    ScaleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Achievements_AchievementScales_ScaleId",
                        column: x => x.ScaleId,
                        principalTable: "AchievementScales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Achievements_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppUserAppointmentType",
                columns: table => new
                {
                    AdditionalInviteesId = table.Column<int>(type: "integer", nullable: false),
                    AppointmentTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserAppointmentType", x => new { x.AdditionalInviteesId, x.AppointmentTypeId });
                    table.ForeignKey(
                        name: "FK_AppUserAppointmentType_AppointmentTypes_AppointmentTypeId",
                        column: x => x.AppointmentTypeId,
                        principalTable: "AppointmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserAppointmentType_Users_AdditionalInviteesId",
                        column: x => x.AdditionalInviteesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserAppointmentType1",
                columns: table => new
                {
                    AppointmentType1Id = table.Column<int>(type: "integer", nullable: false),
                    ExclusiveHostsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserAppointmentType1", x => new { x.AppointmentType1Id, x.ExclusiveHostsId });
                    table.ForeignKey(
                        name: "FK_AppUserAppointmentType1_AppointmentTypes_AppointmentType1Id",
                        column: x => x.AppointmentType1Id,
                        principalTable: "AppointmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserAppointmentType1_Users_ExclusiveHostsId",
                        column: x => x.ExclusiveHostsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BullyReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsPublicReport = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    VictimName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    BullyName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Location = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Details = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Observers = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Actions = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    CreatorId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BullyReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BullyReports_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Organizer = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DurationInHours = table.Column<double>(type: "double precision", nullable: false),
                    Certificate = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsUseful = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FailureReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Details = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ReportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsFixed = table.Column<bool>(type: "boolean", nullable: true),
                    FixDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailureReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FailureReports_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_ObservationLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "ObservationLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Observations_ObservationStudents_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ObservationStudents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Observations_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TokenHash = table.Column<byte[]>(type: "bytea", maxLength: 32, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentReservedDates",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "integer", nullable: false),
                    HostId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentReservedDates", x => new { x.DateId, x.HostId });
                    table.ForeignKey(
                        name: "FK_AppointmentReservedDates_AppointmentDates_DateId",
                        column: x => x.DateId,
                        principalTable: "AppointmentDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentReservedDates_Users_HostId",
                        column: x => x.HostId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: true),
                    AppointmentDateId = table.Column<int>(type: "integer", nullable: false),
                    HostId = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    AttendeeName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    AttendeeEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_AppointmentDates_AppointmentDateId",
                        column: x => x.AppointmentDateId,
                        principalTable: "AppointmentDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_HostId",
                        column: x => x.HostId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    IsHidden = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedTitle = table.Column<string>(type: "text", nullable: false, computedColumnSql: "translate(upper(\"Title\"), 'ĄČĘĖĮŠŲŪŽ', 'ACEEISUUZ')", stored: true),
                    Url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsExternal = table.Column<bool>(type: "boolean", nullable: false),
                    PostId = table.Column<int>(type: "integer", nullable: true),
                    LanguageId = table.Column<string>(type: "character varying(5)", nullable: false),
                    ParentMenuId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Menus_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AchievementAppUser",
                columns: table => new
                {
                    AchievementId = table.Column<int>(type: "integer", nullable: false),
                    AdditionalTeachersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementAppUser", x => new { x.AchievementId, x.AdditionalTeachersId });
                    table.ForeignKey(
                        name: "FK_AchievementAppUser_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementAppUser_Users_AdditionalTeachersId",
                        column: x => x.AdditionalTeachersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AchievementStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    AchievementId = table.Column<int>(type: "integer", nullable: false),
                    AchievementTypeId = table.Column<int>(type: "integer", nullable: false),
                    ClassroomId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AchievementStudents_AchievementTypes_AchievementTypeId",
                        column: x => x.AchievementTypeId,
                        principalTable: "AchievementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementStudents_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementStudents_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObservationObservationOption",
                columns: table => new
                {
                    ObservationsId = table.Column<int>(type: "integer", nullable: false),
                    OptionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationObservationOption", x => new { x.ObservationsId, x.OptionsId });
                    table.ForeignKey(
                        name: "FK_ObservationObservationOption_ObservationOptions_OptionsId",
                        column: x => x.OptionsId,
                        principalTable: "ObservationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ObservationObservationOption_Observations_ObservationsId",
                        column: x => x.ObservationsId,
                        principalTable: "Observations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AchievementAppUser_AdditionalTeachersId",
                table: "AchievementAppUser",
                column: "AdditionalTeachersId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_CreatorId",
                table: "Achievements",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_Date",
                table: "Achievements",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_Date_Id",
                table: "Achievements",
                columns: new[] { "Date", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_ScaleId",
                table: "Achievements",
                column: "ScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementStudents_AchievementId",
                table: "AchievementStudents",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementStudents_AchievementTypeId",
                table: "AchievementStudents",
                column: "AchievementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementStudents_ClassroomId",
                table: "AchievementStudents",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDates_Date",
                table: "AppointmentDates",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDates_TypeId_Date",
                table: "AppointmentDates",
                columns: new[] { "TypeId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentReservedDates_HostId",
                table: "AppointmentReservedDates",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentDateId",
                table: "Appointments",
                column: "AppointmentDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AttendeeEmail",
                table: "Appointments",
                column: "AttendeeEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_HostId",
                table: "Appointments",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueAttendeeDate",
                table: "Appointments",
                columns: new[] { "AttendeeEmail", "AppointmentDateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueHost",
                table: "Appointments",
                columns: new[] { "HostId", "AttendeeEmail" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UniqueHostDate",
                table: "Appointments",
                columns: new[] { "HostId", "AppointmentDateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserAppointmentType_AppointmentTypeId",
                table: "AppUserAppointmentType",
                column: "AppointmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserAppointmentType1_ExclusiveHostsId",
                table: "AppUserAppointmentType1",
                column: "ExclusiveHostsId");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_LanguageId_IsPublished_Order_Id",
                table: "Banners",
                columns: new[] { "LanguageId", "IsPublished", "Order", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Banners_NormalizedTitle",
                table: "Banners",
                column: "NormalizedTitle")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_BullyReports_CreatedAt",
                table: "BullyReports",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BullyReports_CreatorId",
                table: "BullyReports",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_BullyReports_CreatorId_CreatedAt",
                table: "BullyReports",
                columns: new[] { "CreatorId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreatorId",
                table: "Courses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_EndDate",
                table: "Courses",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StartDate",
                table: "Courses",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StartDate_Id",
                table: "Courses",
                columns: new[] { "StartDate", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_FailureReports_CreatorId",
                table: "FailureReports",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FailureReports_IsFixed_Id",
                table: "FailureReports",
                columns: new[] { "IsFixed", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_FailureReports_ReportDate",
                table: "FailureReports",
                column: "ReportDate");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_LanguageId",
                table: "Menus",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_LanguageId_IsPublished_IsHidden_Order_Id",
                table: "Menus",
                columns: new[] { "LanguageId", "IsPublished", "IsHidden", "Order", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_LanguageId_Url_IsPublished",
                table: "Menus",
                columns: new[] { "LanguageId", "Url", "IsPublished" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_NormalizedTitle",
                table: "Menus",
                column: "NormalizedTitle")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Order",
                table: "Menus",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentMenuId",
                table: "Menus",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_PostId",
                table: "Menus",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObservationLessons_Name",
                table: "ObservationLessons",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObservationObservationOption_OptionsId",
                table: "ObservationObservationOption",
                column: "OptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationOptions_Name",
                table: "ObservationOptions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Observations_CreatorId",
                table: "Observations",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_CreatorId_Id",
                table: "Observations",
                columns: new[] { "CreatorId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Observations_Date_Id",
                table: "Observations",
                columns: new[] { "Date", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Observations_LessonId",
                table: "Observations",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_StudentId",
                table: "Observations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_StudentId_Date_Id",
                table: "Observations",
                columns: new[] { "StudentId", "Date", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ObservationStudents_Name",
                table: "ObservationStudents",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsFeatured_PublishedAt",
                table: "Posts",
                columns: new[] { "IsFeatured", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LanguageId_IsPublished_ShowInFeed_IsFeatured_Publishe~",
                table: "Posts",
                columns: new[] { "LanguageId", "IsPublished", "ShowInFeed", "IsFeatured", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_NormalizedTitle",
                table: "Posts",
                column: "NormalizedTitle")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash_ExpiresAt",
                table: "RefreshTokens",
                columns: new[] { "TokenHash", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortDays_Date",
                table: "ShortDays",
                column: "Date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetable_RoomId_DayId",
                table: "Timetable",
                columns: new[] { "RoomId", "DayId" });

            migrationBuilder.CreateIndex(
                name: "IX_Timetable_TimeId",
                table: "Timetable",
                column: "TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableOverrides_Date_RoomId",
                table: "TimetableOverrides",
                columns: new[] { "Date", "RoomId" });

            migrationBuilder.CreateIndex(
                name: "IX_TimetableOverrides_RoomId",
                table: "TimetableOverrides",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableOverrides_TimeId",
                table: "TimetableOverrides",
                column: "TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsTeacher_IsSuspended_Name",
                table: "Users",
                columns: new[] { "IsTeacher", "IsSuspended", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedName",
                table: "Users",
                column: "NormalizedName")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AchievementAppUser");

            migrationBuilder.DropTable(
                name: "AchievementStudents");

            migrationBuilder.DropTable(
                name: "AppointmentReservedDates");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AppUserAppointmentType");

            migrationBuilder.DropTable(
                name: "AppUserAppointmentType1");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "BullyReports");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "FailureReports");

            migrationBuilder.DropTable(
                name: "JobRecord");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "ObservationObservationOption");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "ShortDays");

            migrationBuilder.DropTable(
                name: "Timetable");

            migrationBuilder.DropTable(
                name: "TimetableOverrides");

            migrationBuilder.DropTable(
                name: "AchievementTypes");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "AppointmentDates");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "ObservationOptions");

            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "Classdays");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Classtimes");

            migrationBuilder.DropTable(
                name: "AchievementScales");

            migrationBuilder.DropTable(
                name: "AppointmentTypes");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "ObservationLessons");

            migrationBuilder.DropTable(
                name: "ObservationStudents");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

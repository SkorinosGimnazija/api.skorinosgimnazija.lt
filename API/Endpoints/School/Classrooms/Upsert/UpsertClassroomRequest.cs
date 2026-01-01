namespace API.Endpoints.School.Classrooms.Upsert;

using System.ComponentModel.DataAnnotations;
using API.Database.Entities.School;
using JetBrains.Annotations;

[PublicAPI]
public record UpsertClassroomRequest
{
    public required int Id { get; init; }

    [StringLength(ClassroomConfiguration.NameLength)]
    public required string Name { get; init; }
}
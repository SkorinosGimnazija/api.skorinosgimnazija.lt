namespace SkorinosGimnazija.Domain.Entities.Observation;

public sealed class ObservationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<StudentObservation> StudentObservations { get; set; } = [];
}
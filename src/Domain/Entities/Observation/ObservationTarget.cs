namespace SkorinosGimnazija.Domain.Entities.Observation;

public sealed class ObservationTarget
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public bool Enabled { get; set; }
}
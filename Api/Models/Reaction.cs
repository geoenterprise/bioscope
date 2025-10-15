namespace PlantAnimalApi.Models;

public class Reaction
{
    public Guid ReactionId { get; set; }
    
    public Guid DiscoveryId { get; set; }
    public Discovery Discovery { get; set; } = default!;
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
}

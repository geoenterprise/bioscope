namespace PlantAnimalApi.Models;

public class Comment
{
    public Guid Id { get; set; }
    public Guid DiscoveryId { get; set; }
    public Discovery Discovery { get; set; } = default!;
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public string Body { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

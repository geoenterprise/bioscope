namespace PlantAnimalApi.Contracts;
public class CommentCreateDto
{
    public Guid DiscoveryId { get; set; }
    public Guid UserId { get; set; }
    public string Body { get; set; } = default!;
}

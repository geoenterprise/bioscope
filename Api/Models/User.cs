namespace PlantAnimalApi.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!; 
    public string PasswordHash { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    public ICollection<Discovery> Discoveries { get; set; } = new List<Discovery>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}

using System.Text.Json;

namespace PlantAnimalApi.Models;

public class Discovery
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public string? TopMatchName { get; set; }
    public decimal? Confidence { get; set; }           // numeric(5,4)
    public string? ApiProvider { get; set; }           // default 'plant.id'
    public string? ApiResult { get; set; }             // store JSON as text; DB is jsonb

    public DateTimeOffset? TakenAt { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public bool IsPublic { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<MediaAsset> MediaAssets { get; set; } = new();
    public ICollection<Comment> Comments { get; set; } = new();
    public ICollection<Reaction> Reactions { get; set; } = new();
}

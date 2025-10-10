using System.Text.Json;

namespace PlantAnimalApi.Models;

public class Discovery
{
    public Guid Id { get; set; }
    public string Plant_Name { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public decimal? Confidence { get; set; }           // numeric(5,4) this is the probability
    public string? ApiProvider { get; set; }           // default 'plant.id'
    public string? ApiResult { get; set; }             // store JSON as text; DB is jsonb
    public DateTimeOffset? TakenAt { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsPublic { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ICollection<MediaAsset> MediaAssets { get; set; } = new List<MediaAsset>(); // this is the similar_images 
    public ICollection<Comment> Comments    { get; set; } = new List<Comment>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    public List<string>? Common_Names { get; set; }
    public string? Wiki_Description { get; set; } //I will only pass on the text that comes from Maui
    public string? Scientific_Name { get; set; }
}

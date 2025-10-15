using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantAnimalApi.Models;

public class Discovery
{
    public Guid DiscoveryId { get; set; }
    public string CommonName { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    [Column(TypeName = "numeric(5,4)")] 
    public decimal? Confidence { get; set; }           // numeric(5,4) this is the probability
    public string? AssetUrl { get; set; }  // this is the similar_images 
    public string? WikiDescription { get; set; } //I will only pass on the text that comes from Maui
    public string? ScientificName { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
}

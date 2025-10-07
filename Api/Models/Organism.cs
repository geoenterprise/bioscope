using System.Text.Json.Serialization;

namespace PlantAnimalApi.Models

{
    public class Organism
    {
        public int Id { get; set; }
        public string CommonName { get; set; } = string.Empty;
        public string ScientificName { get; set; } = string.Empty;
        public string Category { get; set; } = "Plant"; // or "Animal"
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class PlantIdResult
    {
        public string? Id { get; set; }
        public List<Suggestion>? Suggestions { get; set; }
    }

    public class Suggestion
    {
        [JsonPropertyName("plant_name")]
        public string? PlantName { get; set; }

        [JsonPropertyName("probability")]
        public double Probability { get; set; }
    }
}

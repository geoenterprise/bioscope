using System.Text.Json.Serialization;

namespace PlantAnimalApi.Models

{

    // [JsonPropertyName("id")]
    //     public long Id { get; set; }

    //     [JsonPropertyName("plant_name")]
    //     public string Plant_Name { get; set; } = string.Empty;

    //     [JsonPropertyName("probability")]
    //     public double Probability { get; set; }

    //     [JsonPropertyName("similar_images")]
    //     public List<SuggestionImages> Similar_Images { get; set; } = new();
    public class Organism
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("plant_name")]
        public string Common_Name { get; set; } = string.Empty;
        public string ScientificName { get; set; } = string.Empty;
        public string Category { get; set; } = "Plant"; // or "Animal"

        [JsonPropertyName("similar_images")]
        public List<SimilarImages> ImageUrl{ get; set; } = new();

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

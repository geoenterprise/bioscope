using System.Text.Json.Serialization;

namespace MobileApp.Models
{
    public class Suggestion
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("plant_name")]
        public string Plant_Name { get; set; } = string.Empty;

        [JsonPropertyName("probability")]
        public double Probability { get; set; }

        [JsonPropertyName("similar_images")]
        public List<SuggestionImages> Similar_Images { get; set; } = new();
    }

}
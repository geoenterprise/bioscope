using System.Text.Json.Serialization;
namespace MobileApp.Models{

    public class PlantDetails
    {
        [JsonPropertyName("common_names")]
        public List<string>? Common_Names { get; set; }

        [JsonPropertyName("taxonomy")]
        public Taxonomy? Taxonomy { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("wiki_description")]
        public WikiDescription? Wiki_Description { get; set; }

        [JsonPropertyName("scientific_name")]
        public string? Scientific_Name { get; set; }

    }
}
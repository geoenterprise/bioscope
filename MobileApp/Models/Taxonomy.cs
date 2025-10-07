using System.Text.Json.Serialization;

namespace MobileApp.Models
{
    public class Taxonomy
    {
        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("genus")]
        public string Genus { get; set; }

        [JsonPropertyName("order")]
        public string Order { get; set; }

        [JsonPropertyName("family")]
        public string Family { get; set; }

        [JsonPropertyName("phylum")]
        public string Phylum { get; set; }

        [JsonPropertyName("kingdom")]
        public string Kingdom { get; set; }
    }

}
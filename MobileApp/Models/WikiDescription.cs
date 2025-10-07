using System.Text.Json.Serialization;

namespace MobileApp.Models
{
    public class WikiDescription
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("citation")]
        public string? Citation { get; set; }
        public string? License_Name { get; set; }
        public string? License_Url { get; set; }
    }

}
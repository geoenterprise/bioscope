using System.Text.Json.Serialization;

namespace MobileApp.Models
{
    public class SimilarImages
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } =string.Empty;

        [JsonPropertyName("url_small")]
        public string Url_Small { get; set; } = string.Empty;

        // puedes agregar más campos si quieres
    }

}

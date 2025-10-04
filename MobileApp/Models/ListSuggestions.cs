using System.Text.Json.Serialization;


namespace MobileApp.Models
{
    public class ListSuggestions
    {
        [JsonPropertyName("suggestions")]
        public List<Suggestion> Suggestions { get; set; } = new();
    }


}


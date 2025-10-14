using System.Text.Json;

namespace MobileApp.Models
{

    public class Discovery
    {
        public Guid DiscoveryId { get; set; }
        public string CommonName { get; set; }
        public Guid UserId { get; set; }

        public decimal? Confidence { get; set; }
        public string? AssetUrl { get; set; }
        public string? WikiDescription { get; set; }
        public string? ScientificName { get; set; }
    }

}
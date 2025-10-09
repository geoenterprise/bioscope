namespace MobileApp.Models{

    public class MediaAsset
    {
        public Guid Id { get; set; }
        public Guid DiscoveryId { get; set; }
        public Discovery Discovery { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string? ThumbUrl { get; set; }
        public string? ContentType { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public long? SizeBytes { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

}

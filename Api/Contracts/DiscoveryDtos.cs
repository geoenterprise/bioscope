namespace PlantAnimalApi.Contracts;

public record CreateDiscoveryRequest(
    string? CommonName,
    decimal? Confidence,
    string? AssetUrl,
    string? ScientificName,
    string? WikiDescription,
    string? Comment
);


public record CreateMediaItem(string Url, string? ThumbUrl, string? ContentType, int? Width, int? Height, long? SizeBytes);

public record CreateCommentRequest(string Body);

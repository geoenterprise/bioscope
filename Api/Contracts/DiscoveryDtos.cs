namespace PlantAnimalApi.Contracts;

public record CreateDiscoveryRequest(
    string? TopMatchName,
    decimal? Confidence,
    string? ApiProvider,
    string? ApiResult,   // JSON string from Plant.id
    DateTimeOffset? TakenAt,
    double? Latitude,
    double? Longitude,
    bool IsPublic,
    List<CreateMediaItem>? Media // optional immediate media entries
);

public record CreateMediaItem(string Url, string? ThumbUrl, string? ContentType, int? Width, int? Height, long? SizeBytes);

public record CreateCommentRequest(string Body);

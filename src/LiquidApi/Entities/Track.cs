namespace LiquidApi.Entities;

public class Track
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string ArtistId { get; init; }
    public required string? AlbumId { get; init; }
    public required int? TrackNumber { get; init; }
}

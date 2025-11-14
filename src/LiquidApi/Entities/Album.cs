namespace LiquidApi.Entities;

public class Album
{
    public required string Id { get; init; }
    public required int ArtistId { get; init; }
    public required string Name { get; init; }
    public required string Genre { get; init; }
    public required uint ReleaseYear { get; init; }
}

namespace LiquidApi.Entities;

public class Album
{
    public required int Id { get; init; }
    public required int ArtistId { get; init; }
    public required string Title { get; init; }
    public required string? Genre { get; init; }
    public required int? ReleaseYear { get; init; }
}

namespace LiquidApi.Entities;

public class Artist
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required IReadOnlyList<string> Genres { get; init; }
    public required uint Popularity { get; init; }
}

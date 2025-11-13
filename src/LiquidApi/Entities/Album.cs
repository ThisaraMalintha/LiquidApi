namespace LiquidApi.Entities;

public class Album
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required uint TrackCount { get; init; }
    public required DateOnly ReleaseDate { get; init; }
}

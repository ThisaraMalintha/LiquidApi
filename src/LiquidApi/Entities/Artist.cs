namespace LiquidApi.Entities;

/// <summary>
/// Entity type of an artist or band
/// </summary>
public class Artist
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Genre { get; init; }
    public required string? Country { get; init; }
    public required int? FormedYear { get; init; }
    public required int? MemberCount { get; init; }
}

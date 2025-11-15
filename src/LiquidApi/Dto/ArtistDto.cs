namespace LiquidApi.Dto;

public class ArtistDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Genre { get; init; }
    public required string? Country { get; init; }
    public required int? FormedYear { get; init; }
    public required int? MemberCount { get; init; }

    public static ArtistDto FromArtist(Artist artist)
    {
        return new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Genre = artist.Genre,
            Country = artist.Country,
            FormedYear = artist.FormedYear,
            MemberCount = artist.MemberCount
        };
    }
}

using System.Text.Json.Serialization;

namespace LiquidApi.Services.MusicApi.Models;

public class TheAudioDbArtistResponse
{
    [JsonPropertyName("artists")]
    public required IReadOnlyList<TheAudioDbArtist> Artists { get; init; }
}

/// <summary>
/// A simplified view of the audio db artist object.
/// </summary>
public class TheAudioDbArtist
{
    [JsonPropertyName("idArtist")]
    public required int Id { get; init; }

    [JsonPropertyName("strArtist")]
    public required string Name { get; init; }

    [JsonPropertyName("strGenre")]
    public required string? Genre { get; init; }

    [JsonPropertyName("strCountry")]
    public required string? Country { get; init; }

    [JsonPropertyName("intFormedYear")]
    public required int? FormedYear { get; init; }

    [JsonPropertyName("intMembers")]
    public required int? MemberCount { get; init; }

    public Artist ToArtist()
    {
        return new Artist
        {
            Id = Id,
            Name = Name,
            Genre = Genre,
            Country = Country,
            // TheMusicDb sometimes return 0 instead of null :(
            FormedYear = FormedYear == 0 ? null : FormedYear,
            MemberCount = MemberCount == 0 ? null : MemberCount
        };
    }
}

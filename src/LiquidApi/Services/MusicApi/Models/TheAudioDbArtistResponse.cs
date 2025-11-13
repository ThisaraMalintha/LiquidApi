using System.Text.Json.Serialization;

namespace LiquidApi.Services.MusicApi.Models;

public class TheAudioDbArtistResponse
{
    [JsonPropertyName("artists")]
    public required IReadOnlyList<TheAudioDbArtist> Artists { get; init; }
}

public class TheAudioDbArtist
{
    [JsonPropertyName("idArtist")]
    public required int Id { get; init; }

    [JsonPropertyName("strArtist")]
    public required string Name { get; init; }

    [JsonPropertyName("strGenre")]
    public required string Genre { get; init; }

    public Artist ToArtist()
    {
        return new Artist
        {
            Id = Id,
            Name = Name,
            Genre = Genre
        };
    }
}

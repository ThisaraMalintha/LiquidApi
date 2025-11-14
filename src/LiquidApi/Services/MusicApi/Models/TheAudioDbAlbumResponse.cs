using System.Text.Json.Serialization;

namespace LiquidApi.Services.MusicApi.Models;

public class TheAudioDbAlbumResponse
{
    [JsonPropertyName("album")]
    public required IReadOnlyList<TheAudioDbAlbum>? Albums { get; init; }
}

/// <summary>
/// Simplified view of the audio db album object.
/// </summary>
public class TheAudioDbAlbum
{
    [JsonPropertyName("idAlbum")]
    public required int Id { get; init; }

    [JsonPropertyName("idArtist")]
    public required int ArtistId { get; init; }

    [JsonPropertyName("strAlbum")]
    public required string Name { get; init; }

    [JsonPropertyName("strGenre")]
    public required string? Genre { get; init; }

    [JsonPropertyName("intYearReleased")]
    public required uint ReleaseYear { get; init; }

    public Album ToAlbumEntity()
    {
        return new Album()
        {
            Id = Id,
            ArtistId = ArtistId,
            Title = Name,
            Genre = Genre ?? "Unknown",
            ReleaseYear = ReleaseYear
        };
    }
}
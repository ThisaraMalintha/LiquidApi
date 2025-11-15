using LiquidApi.Configuration;
using LiquidApi.Services.MusicApi.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace LiquidApi.Services.MusicApi;

public class TheAudioDbApiClient : IMusicApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TheAudioDbConfiguration _audioDbConfig;

    public TheAudioDbApiClient(IHttpClientFactory httpClientFactory,
        IOptions<TheAudioDbConfiguration> audioDbConfig)
    {
        _httpClientFactory = httpClientFactory;
        _audioDbConfig = audioDbConfig.Value ?? throw new ArgumentNullException(nameof(audioDbConfig));
    }

    public async Task<Artist?> GetArtistById(int artistId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(artistId);

        var endpoint = GetAbsoluteUrl($"artist.php?i={artistId}");

        using var httpClient = _httpClientFactory.CreateClient();

        var result = await httpClient.GetAsync(endpoint);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => await ParseArtistResponse(result),
            HttpStatusCode.NotFound or HttpStatusCode.NoContent => null,
            _ => throw new TheAudioDbException("Artist lookup failed")
        };
    }

    public async Task<Artist?> GetArtistByName(string artistName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artistName);

        var endpoint = GetAbsoluteUrl($"search.php?s={artistName}");

        using var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync(endpoint);

        return response.StatusCode switch
        {
            HttpStatusCode.OK => await ParseArtistResponse(response),
            HttpStatusCode.NotFound or HttpStatusCode.NoContent => null,
            _ => throw new TheAudioDbException("Album search request failed.")
        };
    }

    private static async Task<Artist?> ParseArtistResponse(HttpResponseMessage result)
    {
        var artistResult = await result.Content.ReadFromJsonAsync<TheAudioDbArtistResponse>();

        return artistResult?.Artists?.FirstOrDefault()?.ToArtist();
    }

    public async Task<IReadOnlyList<Album>> GetAlbumsByArtist(string artistName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artistName);

        var endpoint = GetAbsoluteUrl($"searchalbum.php?s={artistName}");

        using var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync(endpoint);

        return response.StatusCode switch
        {
            HttpStatusCode.OK => await ParseAlbumsResponse(response),
            HttpStatusCode.NotFound or HttpStatusCode.NoContent => [],
            _ => throw new TheAudioDbException("Album search request failed.")
        };
    }

    private static async Task<IReadOnlyList<Album>> ParseAlbumsResponse(HttpResponseMessage response)
    {
        var albumResponse = await response.Content.ReadFromJsonAsync<TheAudioDbAlbumResponse>()
            ?? throw new TheAudioDbException("Album response parsing failed");

        if (albumResponse?.Albums == null)
        {
            return [];
        }

        return albumResponse.Albums
            .Select(a => a.ToAlbumEntity())
            .ToList();
    }

    private string GetAbsoluteUrl(string path)
    {
        return Path.Join(_audioDbConfig.ApiBaseUrl, path);
    }
}

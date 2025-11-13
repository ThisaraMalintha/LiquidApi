using LiquidApi.Configuration;
using LiquidApi.Services.MusicApi.Models;
using Microsoft.Extensions.Options;

namespace LiquidApi.Services.MusicApi;

public class TheAudioDbClient : IMusicApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TheAudioDbConfiguration _audioDbConfig;

    public TheAudioDbClient(IHttpClientFactory httpClientFactory,
        IOptions<TheAudioDbConfiguration> audioDbConfig)
    {
        _httpClientFactory = httpClientFactory;
        _audioDbConfig = audioDbConfig.Value ?? throw new ArgumentNullException(nameof(audioDbConfig));
    }

    public async Task<Artist?> GetArtistDetails(string artistId)
    {
        var endpoint = GetAbsoluteUrl($"/artist.php?i={artistId}");

        using var httpClient = _httpClientFactory.CreateClient();

        var result = await httpClient.GetAsync(endpoint);

        result.EnsureSuccessStatusCode();

        var artistResult = await result.Content.ReadFromJsonAsync<TheAudioDbArtistResponse>();

        return artistResult?.Artists?.FirstOrDefault()?.ToArtist();
    }

    private string GetAbsoluteUrl(string path)
    {
        return Path.Join(_audioDbConfig.ApiBaseUrl, path);
    }
}

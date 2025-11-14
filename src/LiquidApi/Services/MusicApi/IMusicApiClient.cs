namespace LiquidApi.Services.MusicApi;

public interface IMusicApiClient
{
    /// <summary>
    /// Get the details of a specific artist.
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns></returns>
    Task<Artist?> GetArtistDetails(int artistId);
}

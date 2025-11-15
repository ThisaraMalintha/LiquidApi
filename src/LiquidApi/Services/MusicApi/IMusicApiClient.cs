namespace LiquidApi.Services.MusicApi;

public interface IMusicApiClient
{
    /// <summary>
    /// Get the details of a specific artist.
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns></returns>
    Task<Artist?> GetArtistById(int artistId);

    /// <summary>
    /// Get the details of a specific artist by artist's exact name.
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns></returns>
    Task<Artist?> GetArtistByName(string artistName);

    /// <summary>
    /// Get all the albums of an artist.
    /// </summary>
    /// <param name="artistName"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Album>> GetAlbumsByArtist(string artistName);
}

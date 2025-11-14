using LiquidApi.Dto;

namespace LiquidApi.Services;

public interface IMusicService
{
    /// <summary>
    /// Lookup a single Artist using artist id.
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns></returns>
    Task<ArtistDto?> GetArtist(int artistId);

    /// <summary>
    /// Lookup all albums of an artist.
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns></returns>
    Task<IReadOnlyList<AlbumDto>> GetAlbumsByArtist(int artistId);
}
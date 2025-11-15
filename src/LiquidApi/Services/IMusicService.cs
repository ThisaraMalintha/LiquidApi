using LiquidApi.Dto;

namespace LiquidApi.Services;

public interface IMusicService
{
    /// <summary>
    /// Lookup a single Artist using artist id.
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns>Artist details or null if not found.</returns>
    Task<ArtistDto?> GetArtist(int artistId);

    /// <summary>
    /// Lookup a single artist by name.
    /// </summary>
    /// <param name="artistName"></param>
    /// <returns>Artist details or null if not found.</returns>
    Task<ArtistDto?> GetArtistByName(string artistName);

    /// <summary>
    /// Lookup all albums of an artist.
    /// </summary>
    /// <param name="artistId"></param>
    /// <param name="pagination"></param>
    /// <returns>Paginated set of albums of the artist or empty if no albums found.</returns>
    Task<PaginatedResponseDto<AlbumDto>> GetAlbumsByArtist(int artistId, PaginatedRequestDto pagination);
}
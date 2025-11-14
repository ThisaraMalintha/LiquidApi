
namespace LiquidApi.Services;

public interface IMusicService
{
    Task<IReadOnlyList<Album>> GetAlbumsByArtist(int artistId);
    Task<Artist?> GetArtist(int artistId);
}
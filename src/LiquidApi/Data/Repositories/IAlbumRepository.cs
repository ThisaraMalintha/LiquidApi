namespace LiquidApi.Data.Repositories;

public interface IAlbumRepository
{
    Task<PaginatedResult<Album>> GetAlbumsByArtistId(int artistId,
        int offset, int limit);

    Task SaveAlbums(IEnumerable<Album> albums);
}

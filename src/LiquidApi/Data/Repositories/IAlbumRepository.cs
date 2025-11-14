namespace LiquidApi.Data.Repositories;

public interface IAlbumRepository
{
    Task<IReadOnlyList<Album>> GetAlbumsByArtistId(int artistId);

    Task SaveAlbums(IEnumerable<Album> albums);
}

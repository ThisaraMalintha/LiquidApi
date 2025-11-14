namespace LiquidApi.Data.Repositories;

public interface IArtistRepository
{
    Task<Artist?> GetArtistById(int artistId);
    Task SaveArtist(Artist artist);
}

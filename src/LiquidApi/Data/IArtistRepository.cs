namespace LiquidApi.Data;

public interface IArtistRepository
{
    Task<Artist?> GetArtist(int artistId);

    Task SaveArtist(Artist artist);
}

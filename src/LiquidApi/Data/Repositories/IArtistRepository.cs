namespace LiquidApi.Data.Repositories;

public interface IArtistRepository
{
    Task<Artist?> GetArtistById(int artistId);
    Task SaveArtist(Artist artist);
    Task<Artist?> GetArtistByAlias(ArtistAliasText alias);
    Task SaveArtistAlias(ArtistAlias artistAlias);
    Task<bool> IsArtistExists(int artistId);
}

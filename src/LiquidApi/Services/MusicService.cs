using LiquidApi.Data;
using LiquidApi.Exceptions;
using LiquidApi.Services.MusicApi;

namespace LiquidApi.Services;

public class MusicService : IMusicService
{
    private readonly IMusicApiClient _musicApiClient;
    private readonly IArtistRepository _artistRepository;
    private readonly IAlbumRepository _albumRepository;

    public MusicService(IMusicApiClient musicApiClient,
        IArtistRepository artistRepository,
        IAlbumRepository albumRepository)
    {
        _musicApiClient = musicApiClient;
        _artistRepository = artistRepository;
        _albumRepository = albumRepository;
    }

    public async Task<Artist?> GetArtist(int artistId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(artistId);

        var cachedArtist = await _artistRepository.GetArtist(artistId);

        if (cachedArtist == null)
        {
            // No cache hit
            var audioDbArtist = await _musicApiClient.GetArtistDetails(artistId);

            if (audioDbArtist == null)
            {
                // in a real system we should track the not found responses to
                // prevent any malicious user exploiting repeated api calls to non existent artist ids
                return null;
            }

            await _artistRepository.SaveArtist(audioDbArtist);

            return audioDbArtist;
        }

        return cachedArtist;
    }

    public async Task<IReadOnlyList<Album>> GetAlbumsByArtist(int artistId)
    {
        var artist = await GetArtist(artistId);

        if (artist == null)
        {
            throw new EntityNotFoundException(artistId, "Artist not found");
        }

        var albums = await _albumRepository.GetAlbumsByArtistId(artistId);

        if (!albums.Any())
        {
            albums = await _musicApiClient.GetAlbumsByArtist(artist.Name);

            await _albumRepository.SaveAlbums(albums);
        }

        return albums;
    }
}

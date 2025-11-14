using LiquidApi.Data;
using LiquidApi.Dto;
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

    public async Task<ArtistDto?> GetArtist(int artistId)
    {
        var artist = await GetArtistInternal(artistId);

        return artist == null
            ? null
            : ArtistDto.FromArtist(artist);
    }

    public async Task<IReadOnlyList<AlbumDto>> GetAlbumsByArtist(int artistId)
    {
        var artist = await GetArtistInternal(artistId);

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

        return albums
            .Select(album => AlbumDto.FromArtistAndAlbum(artist, album))
            .ToList();
    }

    private async Task<Artist?> GetArtistInternal(int artistId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(artistId);

        var artist = await _artistRepository.GetArtist(artistId);

        if (artist == null)
        {
            // No cache hit
            artist = await _musicApiClient.GetArtistDetails(artistId);

            if (artist == null)
            {
                // in a real system we should track the not found responses to
                // prevent any malicious user exploiting repeated api calls to non existent artist ids
                return null;
            }

            await _artistRepository.SaveArtist(artist);
        }

        return artist;
    }
}

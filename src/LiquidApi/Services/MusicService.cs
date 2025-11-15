using LiquidApi.Data.Repositories;
using LiquidApi.Dto;
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

    public async Task<PaginatedResponseDto<AlbumDto>> GetAlbumsByArtist(int artistId,
        PaginatedRequestDto pagination)
    {
        var artist = await GetArtistInternal(artistId);

        if (artist == null)
        {
            throw new EntityNotFoundException(artistId, "Artist not found");
        }

        var paginatedAlbums = await _albumRepository.GetAlbumsByArtistId(artistId,
            pagination.Offset,
            pagination.Limit);

        if (paginatedAlbums.TotalCount == 0)
        {
            // TheMusicDb api just returns the full result set without any pagination.
            var albums = await _musicApiClient.GetAlbumsByArtist(artist.Name);

            if (albums.Any())
            {
                await _albumRepository.SaveAlbums(albums);
            }

            var albumPage = albums
                .Skip(pagination.Offset)
                .Take(pagination.Limit)
                .ToList();

            paginatedAlbums = new(albumPage, albums.Count);
        }

        return new PaginatedResponseDto<AlbumDto>
        {
            TotalCount = paginatedAlbums.TotalCount,
            Items = paginatedAlbums.Items
                .Select(album => AlbumDto.FromArtistAndAlbum(artist, album))
                .ToList()
        };
    }

    private async Task<Artist?> GetArtistInternal(int artistId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(artistId);

        var artist = await _artistRepository.GetArtistById(artistId);

        if (artist == null)
        {
            // No cache hit
            artist = await _musicApiClient.GetArtistById(artistId);

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

    public async Task<ArtistDto?> GetArtistByName(string artistName)
    {
        var artist = await _musicApiClient.GetArtistByName(artistName);

        return artist == null
            ? null
            : ArtistDto.FromArtist(artist);
    }
}

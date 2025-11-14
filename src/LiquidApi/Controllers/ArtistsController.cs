using LiquidApi.Dto;
using LiquidApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiquidApi.Controllers;

[Route("api/v1/artists")]
[ApiController]
public class ArtistsController(IMusicService musicService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistDto?>> Get(int id)
    {
        var artist = await musicService.GetArtist(id);

        if (artist == null)
        {
            return NotFound();
        }

        return artist;
    }

    [HttpGet("{artistId}/albums")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetArtistAlbums(int artistId)
    {
        var albums = await musicService.GetAlbumsByArtist(artistId);

        if (!albums.Any())
        {
            return NoContent();
        }

        return new(albums);
    }
}

using LiquidApi.Dto;
using LiquidApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LiquidApi.Controllers;

[Route("api/v1/artists")]
[ApiController]
public class ArtistsController(IMusicService musicService) : ControllerBase
{
    [HttpGet("{artistId}")]
    public async Task<ActionResult<ArtistDto?>> GetArtistById(int artistId)
    {
        var artist = await musicService.GetArtist(artistId);

        if (artist == null)
        {
            return NotFound();
        }

        return artist;
    }

    [HttpGet]
    public async Task<ActionResult<ArtistDto>> GetArtistByName(
        [FromQuery(Name = "name")]
        [Required]
        string artistName)
    {
        var artist = await musicService.GetArtistByName(artistName);

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

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
        [MinLength(1)]
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
    public async Task<ActionResult<PaginatedResponseDto<AlbumDto>>> GetArtistAlbums(int artistId,
        [FromQuery] PaginatedRequestDto pagination)
    {
        var albums = await musicService.GetAlbumsByArtist(artistId, pagination);

        if (albums.Items.Count == 0)
        {
            return NoContent();
        }

        return new(albums);
    }
}

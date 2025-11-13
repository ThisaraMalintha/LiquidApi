using LiquidApi.Services.MusicApi;
using Microsoft.AspNetCore.Mvc;

namespace LiquidApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtistsController : ControllerBase
{
    private readonly IMusicApiClient _musicApiClient;

    public ArtistsController(IMusicApiClient musicApiClient)
    {
        _musicApiClient = musicApiClient;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Artist?>> Get(string id)
    {
        return await _musicApiClient.GetArtistDetails(id);
    }
}

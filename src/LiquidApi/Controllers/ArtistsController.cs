using LiquidApi.Data;
using LiquidApi.Services.MusicApi;
using Microsoft.AspNetCore.Mvc;

namespace LiquidApi.Controllers;

[Route("api/v1/artists")]
[ApiController]
public class ArtistsController : ControllerBase
{
    private readonly IMusicApiClient _musicApiClient;
    private readonly IArtistRepository _artistRepository;

    public ArtistsController(IMusicApiClient musicApiClient,
        IArtistRepository artistRepository)
    {
        _musicApiClient = musicApiClient;
        _artistRepository = artistRepository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Artist?>> Get(int id)
    {
        var cachedArtist = await _artistRepository.GetArtist(id);

        if (cachedArtist == null)
        {
            // No cache hit
            var audioDbArtist = await _musicApiClient.GetArtistDetails(id);

            if (audioDbArtist == null)
            {
                // in a real system we should track the not found responses to
                // prevent any malicious user exploiting repeated api calls to non existent artist ids
                return NotFound();
            }

            await _artistRepository.SaveArtist(audioDbArtist);

            return audioDbArtist;
        }

        return cachedArtist;
    }
}

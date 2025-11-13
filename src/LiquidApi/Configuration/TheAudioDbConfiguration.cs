using System.ComponentModel.DataAnnotations;

namespace LiquidApi.Configuration;

public class TheAudioDbConfiguration
{
    [Required]
    public required string ApiBaseUrl { get; init; }
}

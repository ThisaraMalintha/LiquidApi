using System.ComponentModel.DataAnnotations;

namespace LiquidApi.Dto;

public class PaginatedRequestDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public required int Offset { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int Limit { get; init; }
}

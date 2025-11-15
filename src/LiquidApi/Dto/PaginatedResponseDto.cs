namespace LiquidApi.Dto;

public class PaginatedResponseDto<TDto>
{
    public required int TotalCount { get; init; }
    public required IReadOnlyList<TDto> Items { get; init; }
}

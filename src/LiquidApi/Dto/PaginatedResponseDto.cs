namespace LiquidApi.Dto;

public class PaginatedResponseDto<T>
{
    public required int TotalCount { get; init; }
    public required IReadOnlyList<T> Items { get; init; }

    public static PaginatedResponseDto<T> Empty() => new()
    {
        Items = [],
        TotalCount = 0
    };
}

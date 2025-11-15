namespace LiquidApi.Data;

public record PaginatedResult<T>(IReadOnlyList<T> Items, int TotalCount);

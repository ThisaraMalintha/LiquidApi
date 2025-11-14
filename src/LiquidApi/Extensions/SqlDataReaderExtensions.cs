using Microsoft.Data.SqlClient;

namespace LiquidApi.Extensions;

internal static class SqlDataReaderExtensions
{
    public static int GetInteger(this SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);

        return reader.GetInt32(ordinal);
    }
    
    public static string GetString(this SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);

        return reader.GetString(ordinal);
    }
   
    public static int? GetNullableInt(this SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);

        return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
    }

    public static string? GetNullableString(this SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);

        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }
}

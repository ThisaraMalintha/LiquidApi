using Microsoft.Data.SqlClient;

namespace LiquidApi.Extensions;

internal static class SqlDataReaderExtensions
{
    extension(SqlDataReader reader)
    {
        public int GetInteger(string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.GetInt32(ordinal);
        }

        public string GetString(string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.GetString(ordinal);
        }

        public int? GetNullableInteger(string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
        }

        public string? GetNullableString(string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }
    }
}

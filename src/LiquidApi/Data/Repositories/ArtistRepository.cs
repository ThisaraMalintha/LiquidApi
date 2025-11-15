using Microsoft.Data.SqlClient;

namespace LiquidApi.Data.Repositories;

internal class ArtistRepository(IDbConnectionProvider connectionProvider) : IArtistRepository
{
    public async Task<Artist?> GetArtistById(int artistId)
    {
        const string sql =
            """
            SELECT
                artist_id,
                name,
                genre,
                country,
                formed_year,
                member_count
            FROM
                artist
            WHERE
                artist_id = @id
            """;

        using var connection = connectionProvider.GetConnection();
        using var cmd = new SqlCommand
        {
            Connection = connection,
            CommandText = sql,
            Parameters =
            {
                new SqlParameter("@id", artistId)
            }
        };

        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Artist
        {
            Id = reader.GetInteger("artist_id"),
            Name = reader.GetString("name"),
            Genre = reader.GetNullableString("genre"),
            Country = reader.GetNullableString("country"),
            FormedYear = reader.GetNullableInteger("formed_year"),
            MemberCount = reader.GetNullableInteger("member_count"),
        };

    }

    public async Task SaveArtist(Artist artist)
    {
        const string sql =
            """
            INSERT INTO artist
                (artist_id, name, genre, country, formed_year, member_count)
            VALUES
                (@artistId, @name, @genre, @country, @formedYear, @memberCount)
            """;

        using var connection = connectionProvider.GetConnection();
        using var cmd = new SqlCommand
        {
            Connection = connection,
            CommandText = sql,
            Parameters =
            {
                new SqlParameter("@artistId", artist.Id),
                new SqlParameter("@name", artist.Name),

                new SqlParameter("@genre", 
                    string.IsNullOrWhiteSpace(artist.Genre) ? DBNull.Value : artist.Genre),

                new SqlParameter("@country", 
                    string.IsNullOrWhiteSpace(artist.Country) ? DBNull.Value : artist.Country),

                new SqlParameter("@formedYear", 
                    artist.FormedYear.HasValue ? artist.FormedYear : DBNull.Value),

                new SqlParameter("@memberCount",
                    artist.MemberCount.HasValue ? artist.MemberCount : DBNull.Value),
            }
        };

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}

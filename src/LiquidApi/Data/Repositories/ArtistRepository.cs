using LiquidApi.Extensions;
using Microsoft.Data.SqlClient;

namespace LiquidApi.Data.Repositories;

public class ArtistRepository(IDbConnectionProvider connectionProvider) : IArtistRepository
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
            Genre = reader.GetString("genre"),
            Country = reader.GetString("country"),
            FormedYear = reader.GetInteger("formed_year"),
            MemberCount = reader.GetInteger("member_count"),
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
                new SqlParameter("@genre", artist.Genre),
                new SqlParameter("@country", artist.Country),
                new SqlParameter("@formedYear", artist.FormedYear),
                new SqlParameter("@memberCount", artist.MemberCount),
            }
        };

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}

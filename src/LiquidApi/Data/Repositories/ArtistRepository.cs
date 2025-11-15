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

        return ReadArtist(reader);
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

    public async Task<Artist?> GetArtistByAlias(ArtistAliasText alias)
    {
        const string sql =
            """
            SELECT
              a.artist_id,
              a.name,
              a.genre,
              a.country,
              a.formed_year,
              a.member_count
            FROM
              artist a
            INNER JOIN
              artist_alias aa
                ON a.artist_id = aa.artist_id
            WHERE
              aa.alias = @alias
            """;

        using var connection = connectionProvider.GetConnection();
        using var cmd = new SqlCommand
        {
            Connection = connection,
            CommandText = sql,
            Parameters =
            {
                new SqlParameter("@alias", alias.ToString())
            }
        };

        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return ReadArtist(reader);
    }

    public async Task SaveArtistAlias(ArtistAlias artistAlias)
    {
        const string sql =
            """
            INSERT INTO artist_alias
              (artist_id, alias)
            VALUES
              (@artistId, @alias)
            """;

        using var connection = connectionProvider.GetConnection();
        using var cmd = new SqlCommand
        {
            Connection = connection,
            CommandText = sql,
            Parameters =
            {
                new SqlParameter("@artistId", artistAlias.ArtistId),
                new SqlParameter("@alias", artistAlias.Alias.ToString()),
            }
        };

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> IsArtistExists(int artistId)
    {
        const string sql =
            """
            SELECT
              1 AS [exists]
            FROM
              artist a
            WHERE
              a.artist_id = @artistId
            """;

        using var connection = connectionProvider.GetConnection();
        using var cmd = new SqlCommand
        {
            Connection = connection,
            CommandText = sql,
            Parameters =
            {
                new SqlParameter("@artistId", artistId)
            }
        };

        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        return await reader.ReadAsync();
    }

    private static Artist ReadArtist(SqlDataReader reader)
    {
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
}

using Microsoft.Data.SqlClient;

namespace LiquidApi.Data.Repositories;

public class AlbumRepository(IDbConnectionProvider connectionProvider) : IAlbumRepository
{
    public async Task<PaginatedResult<Album>> GetAlbumsByArtistId(int artistId,
        int offset, int limit)
    {
        const string sql =
            """
            WITH totalCount AS 
            (
              SELECT 
                COUNT(1) AS count
              FROM 
                album
              WHERE
                artist_id = @artistId
            )
            SELECT 
              album_id,
              artist_id,
              title,
              genre,
              release_year,
              (SELECT count FROM totalCount) AS total_count
            FROM
              album
            WHERE
              artist_id = @artistId
            ORDER BY
              album_id
            OFFSET 
              @offset ROWS
            FETCH NEXT 
              @pageSize ROWS ONLY
            """;

        using var connection = connectionProvider.GetConnection();
        using var cmd = new SqlCommand
        {
            CommandText = sql,
            Connection = connection,
            Parameters =
            {
                new SqlParameter("@artistId", artistId),
                new SqlParameter("@offset", offset),
                new SqlParameter("@pageSize", limit)
            }
        };

        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        var albums = new List<Album>();
        var total = 0;

        while (await reader.ReadAsync())
        {
            if (total == 0)
            {
                total = reader.GetInteger("total_count");
            }

            albums.Add(new Album
            {
                Id = reader.GetInteger("album_id"),
                ArtistId = reader.GetInteger("artist_id"),
                Title = reader.GetString("title"),
                Genre = reader.GetNullableString("genre"),
                ReleaseYear = reader.GetNullableInt("release_year")
            });
        }

        return new(albums, total);
    }

    public async Task SaveAlbums(IEnumerable<Album> albums)
    {
        const string sql =
            """
            INSERT INTO album
              (album_id, artist_id, title, genre, release_year)
            VALUES
              (@albumId, @artistId, @title, @genre, @releaseYear)
            """;

        using var connection = connectionProvider.GetConnection();
        await connection.OpenAsync();

        // Creating a transsaction since we need all or nothing caching semantics
        using var transaction = await connection.BeginTransactionAsync();

        foreach (var album in albums)
        {
            using var cmd = new SqlCommand
            {
                CommandText = sql,
                Connection = connection,
                Transaction = transaction as SqlTransaction,
                Parameters =
                {
                    new SqlParameter("@albumId", album.Id),
                    new SqlParameter("@artistId", album.ArtistId),
                    new SqlParameter("@title", album.Title),
                    new SqlParameter("@genre", album.Genre == null ? DBNull.Value : album.Genre),
                    new SqlParameter("@releaseYear", album.ReleaseYear.HasValue ? (int)album.ReleaseYear : DBNull.Value)
                }
            };

            await cmd.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
    }
}
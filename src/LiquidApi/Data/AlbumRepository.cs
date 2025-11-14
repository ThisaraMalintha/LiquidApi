using Microsoft.Data.SqlClient;

namespace LiquidApi.Data;

public class AlbumRepository(IDbConnectionProvider connectionProvider) : IAlbumRepository
{
    public async Task<IReadOnlyList<Album>> GetAlbumsByArtistId(int artistId)
    {
        const string sql =
            """
            SELECT 
              album_id,
              artist_id,
              title,
              genre,
              release_year
            FROM
              album
            WHERE
              artist_id = @artistId
            """;

        using var connection = connectionProvider.GetConnection();
        using var cmd = new SqlCommand
        {
            CommandText = sql,
            Connection = connection,
            Parameters =
            {
                new SqlParameter("@artistId", artistId)
            }
        };

        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        var albums = new List<Album>();

        while (await reader.ReadAsync())
        {
            albums.Add(new Album
            {
                Id = reader.GetInt32(reader.GetOrdinal("album_id")),
                ArtistId = reader.GetInt32(reader.GetOrdinal("artist_id")),
                Title = reader.GetString(reader.GetOrdinal("title")),
                Genre = reader.GetString(reader.GetOrdinal("genre")),
                ReleaseYear = (uint)reader.GetInt32(reader.GetOrdinal("release_year")),
            });
        }
        
        return albums;
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
                    new SqlParameter("@genre", album.Genre),
                    new SqlParameter("@releaseYear", (int)album.ReleaseYear)
                }
            };

            await cmd.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
    }
}
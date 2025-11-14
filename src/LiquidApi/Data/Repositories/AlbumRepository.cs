using LiquidApi.Extensions;
using Microsoft.Data.SqlClient;

namespace LiquidApi.Data.Repositories;

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
                Id = reader.GetInteger("album_id"),
                ArtistId = reader.GetInteger("artist_id"),
                Title = reader.GetString("title"),
                Genre = reader.GetNullableString("genre"),
                ReleaseYear = reader.GetNullableInt("release_year")
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
                    new SqlParameter("@genre", album.Genre == null ? DBNull.Value : album.Genre),
                    new SqlParameter("@releaseYear", album.ReleaseYear.HasValue ? (int)album.ReleaseYear : DBNull.Value)
                }
            };

            await cmd.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
    }
}
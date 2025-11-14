namespace LiquidApi.Dto;

public class AlbumDto
{
    public required int Id { get; init; }
    public required int ArtistId { get; init; }
    public required string ArtistName { get; init; }
    public required string Title { get; init; }
    public required string Genre { get; init; }
    public required int? ReleaseYear { get; init; }

    public static AlbumDto FromArtistAndAlbum(Artist artist, Album album)
    {
        return new AlbumDto
        {
            Id = album.Id,
            ArtistId = artist.Id,
            ArtistName = artist.Name,
            Title = album.Title,
            Genre = album.Genre,
            ReleaseYear = album.ReleaseYear
        };
    }
}

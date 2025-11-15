namespace LiquidApi.Entities;

public class ArtistAlias
{
    public int Id { get; init; }
    public required int ArtistId { get; init; }
    public required ArtistAliasText Alias { get; init; }
}

public record ArtistAliasText
{
    public string Alias { get; }

    public ArtistAliasText(string alias)
    {
        // Enforce lowercase
        Alias = alias.ToLower();
    }

    public override string ToString() => Alias;

    // Implicit casts to make it easier to use as direct strings
    public static implicit operator string(ArtistAliasText text) => text.Alias;
}
namespace LiquidApi.Exceptions;

public class EntityNotFoundException : Exception
{
    public int EntityId { get; }

    public EntityNotFoundException(int entityId, string message) : base(message)
    {
        EntityId = entityId;
    }

}

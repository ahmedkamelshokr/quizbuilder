namespace Domain.Common
{
    public abstract class Entity
    {
        [Key] public long Id { get; private set; }
    }
}
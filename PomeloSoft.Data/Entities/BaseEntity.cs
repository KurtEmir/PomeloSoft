namespace PomeloSoft.Data.Entities;

public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreationDate { get; set; }
    public Guid? CreatorId { get; set; }
    public virtual User? Creator { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public Guid? LastModifierId { get; set; }
    public virtual User? LastModifier { get; set; }
} 
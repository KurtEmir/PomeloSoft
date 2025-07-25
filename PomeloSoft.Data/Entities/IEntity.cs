namespace PomeloSoft.Data.Entities;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
    bool IsActive { get; set; }
    DateTimeOffset CreationDate { get; set; }
    Guid? CreatorId { get; set; }
    DateTimeOffset? UpdatedDate { get; set; }
    Guid? LastModifierId { get; set; }
} 
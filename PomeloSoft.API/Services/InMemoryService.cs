using System.Collections.Concurrent;
using PomeloSoft.Data.Entities;

namespace PomeloSoft.API.Services;

public class InMemoryService<TEntity, TKey> : IService<TEntity, TKey>
    where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
{
    protected readonly List<TEntity> _items;

    public InMemoryService(List<TEntity> initialItems)
    {
        _items = initialItems;
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return Task.FromResult(_items.Where(i => i.IsActive).AsEnumerable());
    }

    public Task<TEntity?> GetByIdAsync(TKey id)
    {
        var item = _items.FirstOrDefault(i => i.Id.Equals(id) && i.IsActive);
        return Task.FromResult(item);
    }

    public Task<IEnumerable<TEntity>> GetAllInactiveAsync()
    {
        return Task.FromResult(_items.Where(i => !i.IsActive).AsEnumerable());
    }

    public virtual Task<ServiceResponse<TEntity>> CreateAsync(TEntity entity, Guid creatorId)
    {
        if (entity is BaseEntity<TKey> baseEntity)
        {
            baseEntity.CreationDate = DateTimeOffset.UtcNow;
            baseEntity.CreatorId = creatorId;
        }
        entity.IsActive = true;
        _items.Add(entity);
        return Task.FromResult(ServiceResponse<TEntity>.CreateSuccess(entity));
    }

    public virtual Task<ServiceResponse<TEntity?>> UpdateAsync(TKey id, TEntity entity, Guid modifierId)
    {
        var existingItem = _items.FirstOrDefault(i => i.Id.Equals(id));
        if (existingItem == null)
        {
            return Task.FromResult(ServiceResponse<TEntity?>.CreateFailure("Item not found."));
        }

        if (existingItem is BaseEntity<TKey> baseEntity)
        {
            baseEntity.UpdatedDate = DateTimeOffset.UtcNow;
            baseEntity.LastModifierId = modifierId;
        }

        var index = _items.IndexOf(existingItem);
        _items[index] = entity;
        
        return Task.FromResult(ServiceResponse<TEntity?>.CreateSuccess(entity));
    }

    public Task<ServiceResponse<bool>> DeleteAsync(TKey id, Guid modifierId)
    {
        var item = _items.FirstOrDefault(i => i.Id.Equals(id));
        if (item == null)
        {
            return Task.FromResult(ServiceResponse<bool>.CreateFailure("Item not found."));
        }

        item.IsActive = false; 
        if (item is BaseEntity<TKey> baseEntity)
        {
            baseEntity.UpdatedDate = DateTimeOffset.UtcNow;
            baseEntity.LastModifierId = modifierId;
        }
        return Task.FromResult(ServiceResponse<bool>.CreateSuccess(true));
    }

    public Task<ServiceResponse<bool>> RestoreAsync(TKey id, Guid modifierId)
    {
        var item = _items.FirstOrDefault(i => i.Id.Equals(id));
        if (item == null)
        {
            return Task.FromResult(ServiceResponse<bool>.CreateFailure("Item not found."));
        }
        item.IsActive = true;
        return Task.FromResult(ServiceResponse<bool>.CreateSuccess(true));
    }
} 
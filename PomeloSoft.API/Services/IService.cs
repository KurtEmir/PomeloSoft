using PomeloSoft.Data.Entities;

namespace PomeloSoft.API.Services;

public interface IService<TEntity, TKey> where TEntity : IEntity<TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllInactiveAsync();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<ServiceResponse<TEntity>> CreateAsync(TEntity entity, Guid creatorId);
    Task<ServiceResponse<TEntity?>> UpdateAsync(TKey id, TEntity entity, Guid modifierId);
    Task<ServiceResponse<bool>> DeleteAsync(TKey id, Guid modifierId);
    Task<ServiceResponse<bool>> RestoreAsync(TKey id, Guid modifierId);
} 
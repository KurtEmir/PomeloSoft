using PomeloSoft.Data.Entities;

namespace PomeloSoft.API.Services;

public interface IProductService : IService<Product, int>
{
    Task<ServiceResponse<bool>> DeleteByNameAsync(string name, Guid modifierId);
} 
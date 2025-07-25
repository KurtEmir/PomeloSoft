using PomeloSoft.API.Data;
using PomeloSoft.Data.Entities;

namespace PomeloSoft.API.Services;

public class InMemoryProductService : InMemoryService<Product, int>, IProductService
{
    public InMemoryProductService() : base(DataSeeder.SeedProducts())
    {
    }

    public override Task<ServiceResponse<Product>> CreateAsync(Product product, Guid creatorId)
    {
        var normalizedNewName = new string(product.Name.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLower();
        if (_items.Any(p => new string(p.Name.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLower() == normalizedNewName))
        {
            return Task.FromResult(ServiceResponse<Product>.CreateFailure("You already have a product which has the same name."));
        }

        var newId = _items.Any() ? _items.Max(p => p.Id) + 1 : 1;
        product.Id = newId;
        
        return base.CreateAsync(product, creatorId);
    }

    public Task<ServiceResponse<bool>> DeleteByNameAsync(string name, Guid modifierId)
    {
        var item = _items.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && p.IsActive);
        if (item == null)
        {
            return Task.FromResult(ServiceResponse<bool>.CreateFailure("Product not found."));
        }

        item.IsActive = false;
        if (item is BaseEntity<int> baseEntity)
        {
            baseEntity.UpdatedDate = DateTimeOffset.UtcNow;
            baseEntity.LastModifierId = modifierId;
        }
        
        return Task.FromResult(ServiceResponse<bool>.CreateSuccess(true));
    }
} 
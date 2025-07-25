using PomeloSoft.API.Data;
using PomeloSoft.Data.Entities;

namespace PomeloSoft.API.Services;

public class InMemoryCategoryService : InMemoryService<Category, int>, ICategoryService
{
    public InMemoryCategoryService() : base(DataSeeder.SeedCategories())
    {
    }

     public override Task<ServiceResponse<Category>> CreateAsync(Category category, Guid creatorId)
    {
        var newId = _items.Any() ? _items.Max(p => p.Id) + 1 : 1;
        category.Id = newId;
     
        category.CreationDate = DateTimeOffset.UtcNow;
        category.IsActive = true;
        category.CreatorId = Guid.NewGuid();

        _items.Add(category);
        return Task.FromResult(ServiceResponse<Category>.CreateSuccess(category));
    }
} 
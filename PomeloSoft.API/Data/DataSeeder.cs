using PomeloSoft.Data.Entities;

namespace PomeloSoft.API.Data;

public static class DataSeeder
{
    public static List<Category> SeedCategories()
    {
        return new List<Category>
        {
            new Category { Id = 1, Name = "Laptops", IsActive = true, CreationDate = DateTimeOffset.UtcNow },
            new Category { Id = 2, Name = "Phones", IsActive = true, CreationDate = DateTimeOffset.UtcNow },
            new Category { Id = 3, Name = "I/O Devices", IsActive = true, CreationDate = DateTimeOffset.UtcNow },
            new Category { Id = 4, Name = "Tablets", IsActive = true, CreationDate = DateTimeOffset.UtcNow },
        };
    }

    public static List<Product> SeedProducts()
    {
        return new List<Product>
        {
            // Laptops
            new Product 
            { 
                Id = 1, Name = "MacBook Pro 14", Brand = "Apple", Description = "A powerful laptop for professionals", 
                ImageThumbnailUrl = "https://productimages.hepsiburada.net/s/49/375-375/10983949860914.jpg",
                Price = 2399.99m, Stock = 15, CategoryId = 1, IsActive = true, CreationDate = DateTimeOffset.UtcNow
            },
            new Product 
            { 
                Id = 2, Name = "XPS 15", Brand = "Dell", Description = "Stunning display and performance", 
                ImageThumbnailUrl = "https://m.media-amazon.com/images/I/51oj7-YIfAL._UF1000,1000_QL80_.jpg",
                Price = 2199.50m, Stock = 20, CategoryId = 1, IsActive = true, CreationDate = DateTimeOffset.UtcNow
            },
            new Product 
            { 
                Id = 3, Name = "iPhone 15 Pro", Brand = "Apple", Description = "The latest and greatest from Apple", 
                ImageThumbnailUrl = "https://m.media-amazon.com/images/I/81-hBEU+ZdL._UF1000,1000_QL80_.jpg",
                Price = 999.00m, Stock = 50, CategoryId = 2, IsActive = true, CreationDate = DateTimeOffset.UtcNow
            },
            new Product 
            { 
                Id = 4, Name = "Galaxy S24 Ultra", Brand = "Samsung", Description = "A powerful Android flagship with a stylus", 
                ImageThumbnailUrl = "https://cdn.dsmcdn.com/mnresize/420/620/ty1116/product/media/images/prod/PIM/20240103/11/a6a24430-6894-406b-9e1c-4dafa7972cae/1_org_zoom.jpg",
                Price = 1299.00m, Stock = 40, CategoryId = 2, IsActive = true, CreationDate = DateTimeOffset.UtcNow
            },
            new Product
            {
                Id = 5, Name = "Pixel 8", Brand = "Google", Description = "The purest Android experience",
                ImageThumbnailUrl = "https://m.media-amazon.com/images/I/71SfoZu9a3L._AC_SL1500_.jpg",
                Price = 699.00m, Stock = 60, CategoryId = 2, IsActive = false, CreationDate = DateTimeOffset.UtcNow 
            },
            new Product 
            { 
                Id = 6, Name = "MX Master 3S", Brand = "Logitech", Description = "A high-precision wireless mouse", 
                ImageThumbnailUrl = "https://cdn.akakce.com/_static/1961332076/logitech-mx-master-3s-sarjli.jpg",
                Price = 99.99m, Stock = 100, CategoryId = 3, IsActive = true, CreationDate = DateTimeOffset.UtcNow
            },
            new Product 
            { 
                Id = 7, Name = "G Pro X Keyboard", Brand = "Logitech", Description = "A mechanical gaming keyboard",
                ImageThumbnailUrl = "https://resource.logitechg.com/d_transparent.gif/content/dam/gaming/en/products/pro-keyboard/pro-clicky-gallery-1.png",
                Price = 149.50m, Stock = 75, CategoryId = 3, IsActive = true, CreationDate = DateTimeOffset.UtcNow
            },
            new Product
            {
                Id = 8, Name = "iPad Air", Brand = "Apple", Description = "Thin, light, and powerful",
                ImageThumbnailUrl = "https://m.media-amazon.com/images/I/61ULwAcGldL._UF1000,1000_QL80_.jpg",
                Price = 599.00m, Stock = 30, CategoryId = 4, IsActive = true, CreationDate = DateTimeOffset.UtcNow
            },
             new Product
            {
                Id = 9, Name = "Galaxy Tab S9 FE", Brand = "Samsung", Description = "Thin, light, and powerful",
                ImageThumbnailUrl = "https://www.gizerler.com/assets/content-images/141312/samsung-galaxy-tab-s9-fe-sm-x510nzaatur-109-128-gb-gri-tablet.jpg",
                Price = 599.00m, Stock = 30, CategoryId = 4, IsActive = true, CreationDate = DateTimeOffset.UtcNow
            }
        };
    }
} 
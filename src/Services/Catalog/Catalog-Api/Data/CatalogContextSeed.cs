using Catalog_Api.Entities;
using MongoDB.Driver;

namespace Catalog_Api.Data;

public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> products)
    {
        bool existProduct = products.Find(p => true).Any();
        if (!existProduct)
        {
            products.InsertManyAsync(GetPreconfiguredProducts());
        }
    }

    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        return new List<Product>()
        {
            new Product
            {
                Id = "4bf0bf2bd21041f29ba2576d",
                Name = "iPhone 15 Pro",
                Summary = "iPhone 巅峰之作",
                Description = "Description",
                ImageFile = "iphone15pro.png",
                Price = 7999,
                Category = "Smart Phone"
            },
             new Product
            {
                Id = "4bf0bf2bd21041f29ba25761",
                Name = "iPhone 15",
                Summary = "实打实的实力",
                Description = "Description",
                ImageFile = "iphone15.png",
                Price = 5999,
                Category = "Smart Phone"
            },
              new Product
            {
                Id = "4bf0bf2bd21041f29ba25762",
                Name = "iPhone 14",
                Summary = "浑身都出彩",
                Description = "Description",
                ImageFile = "iphone14.png",
                Price = 5399,
                Category = "Smart Phone"
            },
               new Product
            {
                Id = "4bf0bf2bd21041f29ba25763",
               Name = "iPhone 13",
                Summary = "妙不可言",
                Description = "Description",
                ImageFile = "iphone13.png",
                Price = 4699,
                Category = "Smart Phone"
            }
        };
    }
}

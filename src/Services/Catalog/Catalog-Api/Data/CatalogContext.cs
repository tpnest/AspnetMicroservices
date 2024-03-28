using Catalog_Api.Entities;
using MongoDB.Driver;

namespace Catalog_Api.Data;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        Products = db.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        // 填充种子数据
        CatalogContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}

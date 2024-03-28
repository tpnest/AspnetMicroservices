using Catalog_Api.Data;
using Catalog_Api.Entities;
using MongoDB.Driver;

namespace Catalog_Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task CreateProductAsync(Product product)
    {
        await _context.Products.InsertOneAsync(product);
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var deleteResult = await _context
                                            .Products
                                            .DeleteOneAsync(p => p.Id == id);
        return deleteResult.IsAcknowledged &&
            deleteResult.DeletedCount > 0;
    }

    public async Task<Product> GetProductAsync(string id)
    {
        return await _context
                        .Products
                        .Find(p => p.Id == id)
                        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(string? query = null)
    {

        return await _context
            .Products
            .Find(p => string.IsNullOrWhiteSpace(query)
            || (p.Name.Contains(query) || p.Category.Contains(query)))
            .ToListAsync();

    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var updateResult = await _context
                                                .Products
                                                .ReplaceOneAsync(p => p.Id == product.Id, product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }
}

using Catalog_Api.Entities;

namespace Catalog_Api.Repositories;

public interface IProductRepository
{
    Task<Product> GetProductAsync(string id);
    Task<IEnumerable<Product>> GetProductsAsync(string? query);

    Task CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(string id);
}

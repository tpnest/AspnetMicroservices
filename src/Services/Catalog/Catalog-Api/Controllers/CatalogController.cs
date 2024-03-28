using Catalog_Api.Entities;
using Catalog_Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ILogger<CatalogController> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? query)
    {
        var products = await _productRepository.GetProductsAsync(query);
        return Ok(products);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Product>> GetProduct(string id)
    {
        var product = await _productRepository.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        await _productRepository.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProduct([FromBody] Product product)
    {
        return Ok(await _productRepository.UpdateProductAsync(product));
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        return Ok(await _productRepository.DeleteProductAsync(id));
    }

}

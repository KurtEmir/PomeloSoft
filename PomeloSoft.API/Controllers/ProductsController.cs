using Microsoft.AspNetCore.Mvc;
using PomeloSoft.API.Dtos;
using PomeloSoft.API.Services;
using PomeloSoft.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace PomeloSoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get All Active Products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        _logger.LogInformation("Getting all products");
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("inActive")]
    [SwaggerOperation(Summary = "Get All Inactive Products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetInActiveProducts()
    {
        _logger.LogInformation("Getting all inactive products");
        var products = await _productService.GetAllInactiveAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get Product by ID")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        _logger.LogInformation("Getting product with id {ProductId}", id);
        var product = await _productService.GetByIdAsync(id);
        
        if (product == null)
        {
            _logger.LogWarning("Product with id {ProductId} not found.", id);
            return NotFound($"Product with ID {id} not found.");
        }

        return Ok(product);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a New Product")]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductDto productDto)
    {
        _logger.LogInformation("Creating a new product with name {ProductName}", productDto.Name);
        // DTO to Entity mapping
        var newProduct = new Product
        {
            Name = productDto.Name,
            Brand = productDto.Brand,
            Description = productDto.Description,
            Price = productDto.Price,
            Stock = productDto.Stock,
            ImageThumbnailUrl = productDto.ImageThumbnailUrl,
            CategoryId = productDto.CategoryId
        };

        var creatorId = Guid.NewGuid(); 
        var response = await _productService.CreateAsync(newProduct, creatorId);

        if (!response.Success)
        {
            _logger.LogError("Failed to create product: {ErrorMessage}", response.Message);
            return BadRequest(response.Message);
        }
        
        _logger.LogInformation("Product {ProductName} created successfully with id {ProductId}", newProduct.Name,
            response.Data!.Id);
        return CreatedAtAction(nameof(GetProduct), new { id = response.Data!.Id }, response.Data);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a Product")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto productDto)
    {
        _logger.LogInformation("Updating product with id {ProductId}", id);

        var existingProduct = await _productService.GetByIdAsync(id);
        if (existingProduct == null)
        {
            _logger.LogWarning("Product with id {ProductId} not found for update.", id);
            return NotFound($"Product with ID {id} not found.");
        }
        existingProduct.Description = productDto.Description;
        existingProduct.Price = productDto.Price;
        existingProduct.Stock = productDto.Stock;
        existingProduct.ImageThumbnailUrl = productDto.ImageThumbnailUrl;
        existingProduct.CategoryId = productDto.CategoryId;
        
        var modifierId = Guid.NewGuid();
        var response = await _productService.UpdateAsync(id, existingProduct, modifierId);

        if (!response.Success)
        {
            _logger.LogError("Failed to update product with id {ProductId}. Message: {ErrorMessage}", id,
                response.Message);
            return BadRequest(response.Message);
        }

        _logger.LogInformation("Product with id {ProductId} updated successfully.", id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deactivate a Product (Soft Delete)")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        _logger.LogInformation("Deleting product with id {ProductId}", id);
        var modifierId = Guid.NewGuid();
        var response = await _productService.DeleteAsync(id, modifierId);
        
        if (!response.Success)
        {
            _logger.LogError("Failed to delete product with id {ProductId}: {ErrorMessage}", id, response.Message);
            return NotFound(response.Message);
        }

        _logger.LogInformation("Product with id {ProductId} deleted successfully.", id);
        return NoContent();
    }


    [HttpPut("{id}/restore")]
    [SwaggerOperation(Summary = "Restore a Product")]
    public async Task<IActionResult> RestoreProduct(int id)
    {
        _logger.LogInformation("Restoring product with id {ProductId}", id);
        var modifierId = Guid.NewGuid();
        var response = await _productService.RestoreAsync(id, modifierId);
        if (!response.Success)
        {
            _logger.LogError("Failed to restore product with id {ProductId}: {ErrorMessage}", id, response.Message);
            return NotFound(response.Message);
        }
        _logger.LogInformation("Product with id {ProductId} restored successfully.", id);
        return NoContent();
    }





}


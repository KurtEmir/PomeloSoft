using Microsoft.AspNetCore.Mvc;
using PomeloSoft.API.Dtos;
using PomeloSoft.API.Services;
using PomeloSoft.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace PomeloSoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get All Categories", Description = "Retrieves a list of all active categories.")]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        _logger.LogInformation("Getting all categories");
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get Category by ID", Description = "Retrieves a specific category by its unique ID.")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        _logger.LogInformation("Getting category with id {CategoryId}", id);
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            _logger.LogWarning("Category with id {CategoryId} not found.", id);
            return NotFound($"Category with ID {id} not found.");
        }
        return Ok(category);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a New Category", Description = "Adds a new category to the system.")]
    public async Task<ActionResult<Category>> CreateCategory(CreateCategoryDto categoryDto)
    {
        if (categoryDto == null)
        {
            _logger.LogError("Category data cannot be null.");
            return BadRequest("Category data cannot be null.");
        }
        
        _logger.LogInformation("Creating a new category with name {CategoryName}", categoryDto.Name);

        var newCategory = new Category
        {
            Name = categoryDto.Name
        };

        var createdCategory = await _categoryService.CreateAsync(newCategory, Guid.NewGuid());
        
        if (!createdCategory.Success)
        {
            _logger.LogError("Failed to create category: {ErrorMessage}", createdCategory.Message);
            return BadRequest(createdCategory.Message);
        }

        _logger.LogInformation("Category {CategoryName} created successfully with id {CategoryId}", newCategory.Name, createdCategory.Data!.Id);
        return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Data?.Id }, createdCategory);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a Category", Description = "Updates an existing category's details.")]
    public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto categoryDto)
    {
        if (categoryDto == null)
        {
             _logger.LogError("Category data cannot be null for update.");
            return BadRequest("Category data cannot be null.");
        }
        
        _logger.LogInformation("Updating category with id {CategoryId}", id);
        var existingCategory = await _categoryService.GetByIdAsync(id);
        if (existingCategory == null)
        {
            _logger.LogWarning("Category with id {CategoryId} not found for update.", id);
            return NotFound($"Category with ID {id} not found.");
        }

        existingCategory.Name = categoryDto.Name;

        var response = await _categoryService.UpdateAsync(id, existingCategory, Guid.NewGuid());
        
        if (!response.Success)
        {
            _logger.LogError("Failed to update category with id {CategoryId}. Message: {ErrorMessage}", id, response.Message);
            return BadRequest(response.Message);
        }

        _logger.LogInformation("Category with id {CategoryId} updated successfully.", id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deactivate a Category", Description = "Marks a category as inactive (soft delete).")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        _logger.LogInformation("Deleting category with id {CategoryId}", id);
        var response = await _categoryService.DeleteAsync(id, Guid.NewGuid());
        if (!response.Success)
        {
            _logger.LogError("Failed to delete category with id {CategoryId}: {ErrorMessage}", id, response.Message);
            return NotFound($"Category with ID {id} could not be found to delete.");
        }

        _logger.LogInformation("Category with id {CategoryId} deleted successfully.", id);
        return NoContent();
    }
} 
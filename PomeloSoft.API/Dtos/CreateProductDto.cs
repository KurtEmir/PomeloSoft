using System.ComponentModel.DataAnnotations;

namespace PomeloSoft.API.Dtos;

public class CreateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(200, ErrorMessage = "Product name cannot be longer than 200 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Brand is required.")]
    [MaxLength(100, ErrorMessage = "Brand name cannot be longer than 100 characters.")]
    public string Brand { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 1000000.00, ErrorMessage = "Price must be between 0.01 and 1,000,000.00.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock quantity is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; }
    
    [Url]
    [MaxLength(1000)]
    public string ImageThumbnailUrl { get; set; } = null!;

    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }
} 
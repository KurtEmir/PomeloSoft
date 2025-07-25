using System.ComponentModel.DataAnnotations;

namespace PomeloSoft.API.Dtos;

public class UpdateProductDto
{
    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, 1000000.00)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    
    [Url]
    [MaxLength(1000)]
    public string ImageThumbnailUrl { get; set; } = null!;

    [Required]
    public int CategoryId { get; set; }
} 
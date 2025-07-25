using System.ComponentModel.DataAnnotations;

namespace PomeloSoft.API.Dtos;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
} 
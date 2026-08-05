using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models;

public class CreateGameRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 50)]
    public int GenreId { get; set; }

    [Range(typeof(decimal), "1", "100")]
    public decimal Price { get; set; }

    [Required]
    public DateOnly ReleaseDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryApi.Entities;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int AuthorId { get; set; }

    [ForeignKey("AuthorId")]
    public Author? Author { get; set; }

    [Required]
    [MaxLength(100)]
    public string Genre { get; set; } = string.Empty;

    public int Year { get; set; }

    public string? ImageUrl { get; set; }
}
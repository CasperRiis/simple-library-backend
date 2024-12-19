using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Entities;

public class Author
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Nationality { get; set; } = string.Empty;

    public int BirthYear { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
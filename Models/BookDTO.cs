namespace LibraryApi.Models;

public class BookDTO
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int AuthorId { get; set; }
    public bool IsHidden { get; set; }

    public string Genre { get; set; } = string.Empty;

    public int Year { get; set; }

    public string? ImageUrl { get; set; }
}
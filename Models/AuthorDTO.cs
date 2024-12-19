namespace LibraryApi.Models;

public class AuthorDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Nationality { get; set; } = string.Empty;

    public int BirthYear { get; set; }

    public string? ImageUrl { get; set; }
}
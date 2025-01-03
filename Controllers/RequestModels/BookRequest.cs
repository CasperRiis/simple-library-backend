namespace LibraryApi.RequestModels;

public class BookRequest
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int AuthorId { get; set; }
    public bool IsHidden { get; set; }

    public string Genre { get; set; } = string.Empty;

    public int Year { get; set; }

    public IFormFile? Image { get; set; }
}
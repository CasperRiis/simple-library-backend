namespace LibraryApi.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }
    public string Genre { get; set; }
    public int Year { get; set; }
}

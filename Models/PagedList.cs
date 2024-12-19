namespace LibraryApi.Models;

public class PagedList<T>
{
    public int? Count { get; set; }
    public int? Next { get; set; }
    public List<T>? Results { get; set; }
}
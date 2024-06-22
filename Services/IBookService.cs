using LibraryApi.Models;

namespace LibraryApi.Services
{
    public interface IBookService
    {
        Task<PagedList<Book>> GetBooks(int page, int pageSize, string? searchGenre);
        Task<Book> GetBook(int id);
        Task<Book> GetBook(string bookTitle);
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task DeleteBook(int id);
    }
}
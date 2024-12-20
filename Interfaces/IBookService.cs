using LibraryApi.Models;
using LibraryApi.Entities;

namespace LibraryApi.Interfaces
{
    public interface IBookService
    {
        Task<PagedList<BookDTO_NestedAuthor>> GetBooks(int page, int pageSize, string? searchParameter, string searchProperty = "Genre");
        Task<BookDTO_NestedAuthor> GetBook(int id);
        Task<Book> GetBook(string bookTitle);
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<Book> DeleteBook(int id);
    }
}
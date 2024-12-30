using LibraryApi.Models;
using LibraryApi.Entities;

namespace LibraryApi.Interfaces
{
    public interface IBookService
    {
        Task<PagedList<BookDTO_NestedAuthor>> GetBooks(int page, int pageSize, string? searchParameter, string searchProperty = "Genre");
        Task<BookDTO_NestedAuthor> GetBook(int id);
        Task<BookDTO_NestedAuthor> GetBook(string searchParameter, string searchProperty = "Genre");
        Task<BookDTO_NestedAuthor> AddBook(Book book);
        Task<BookDTO_NestedAuthor> AddBookNoLinq(Book book);
        Task<BookDTO_NestedAuthor> UpdateBook(Book book);
        Task<BookDTO_NestedAuthor> DeleteBook(int id);
    }
}
using LibraryApi.Models;
using LibraryApi.Entities;

namespace LibraryApi.Interfaces
{
    public interface IAuthorService
    {
        Task<PagedList<AuthorDTO_NestedBooks>> GetAuthors(int page, int pageSize, string? searchParameter, string searchProperty = "Name");
        Task<AuthorDTO_NestedBooks> GetAuthor(int id);
        Task<Author> GetAuthor(string authorName);
        Task<Author> AddAuthor(Author author);
        Task<Author> UpdateAuthor(Author author);
        Task<Author> DeleteAuthor(int id);
    }
}
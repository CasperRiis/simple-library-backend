using LibraryApi.Models;
using LibraryApi.Entities;

namespace LibraryApi.Interfaces
{
    public interface IAuthorService
    {
        Task<PagedList<AuthorDTO_NestedBooks>> GetAuthors(int page, int pageSize, string? searchParameter, string searchProperty = "Name");
        Task<AuthorDTO_NestedBooks> GetAuthor(int id);
        Task<AuthorDTO_NestedBooks> GetAuthor(string searchParameter, string searchProperty = "Name");
        Task<AuthorDTO_NestedBooks> AddAuthor(Author author);
        Task<AuthorDTO_NestedBooks> UpdateAuthor(Author author);
        Task<AuthorDTO_NestedBooks> DeleteAuthor(int id);
    }
}
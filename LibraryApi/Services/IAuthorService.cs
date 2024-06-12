using LibraryApi.Models;

namespace LibraryApi.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAuthors();
        Task<Author> GetAuthor(int id);
        Task<Author> GetAuthor(string authorName);
        Task<Author> AddAuthor(Author author);
        Task<Author> UpdateAuthor(Author author);
        Task DeleteAuthor(int id);
    }
}

using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Managers;

public class AuthorDbManager : IAuthorService
{
    private readonly AuthorDbContext _context;

    public AuthorDbManager(AuthorDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAuthors()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author> GetAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        return author != null ? author : throw new ArgumentNullException(nameof(author));
    }

    public async Task<Author> GetAuthor(string authorName)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name!.ToLower().Contains(authorName.ToLower()));
        return author != null ? author : throw new ArgumentNullException(nameof(author));
    }

    public async Task<Author> AddAuthor(Author author)
    {
        if (author.Name == null || author.Nationality == null)
        {
            throw new ArgumentNullException(nameof(author));
        }

        int dbSize = (await _context.Authors.ToListAsync()).LastOrDefault()?.Id ?? 0;
        author.Id = dbSize + 1;

        if (_context.Authors.Count(a => a.Name == author.Name) > 0)
        {
            throw new Exception("Author with same name already exists");
        }

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return author;
    }

    public async Task DeleteAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }

    public async Task<Author> UpdateAuthor(Author author)
    {
        if (_context.Authors.Count(a => a.Name == author.Name && a.Id != author.Id) > 0)
        {
            throw new Exception("Author with same name and different id already exists");
        }

        var dbAuthor = await _context.Authors.FindAsync(author.Id);
        if (dbAuthor == null)
        {
            throw new ArgumentNullException(nameof(dbAuthor));
        }
        _context.Entry(dbAuthor).CurrentValues.SetValues(author);
        await _context.SaveChangesAsync();
        return dbAuthor;
    }
}
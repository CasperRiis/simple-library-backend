using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Managers;

public class BookDbManager : IBookService
{
    private readonly BookDbContext _context;

    public BookDbManager(BookDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetBooks()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book> GetBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        return book != null ? book : throw new ArgumentNullException(nameof(book));
    }

    public async Task<Book> GetBook(string bookTitle)
    {
        var book = await _context.Books.FirstOrDefaultAsync(a => a.Title!.ToLower().Contains(bookTitle.ToLower()));
        return book != null ? book : throw new ArgumentNullException(nameof(book));
    }

    public async Task<Book> AddBook(Book book)
    {
        int dbSize = (await _context.Books.ToListAsync()).LastOrDefault()?.Id ?? 0;
        book.Id = dbSize + 1;

        if (_context.Books.Count(a => a.Title == book.Title) > 0)
        {
            throw new Exception("Book with same title already exists");
        }

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return book;
    }

    public async Task DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            throw new ArgumentNullException(nameof(book));
        }
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }

    public async Task<Book> UpdateBook(Book book)
    {
        if (_context.Books.Count(a => a.Title == book.Title && a.Id != book.Id) > 0)
        {
            throw new Exception("Book with same title and different id already exists");
        }

        var dbBook = await _context.Books.FindAsync(book.Id);
        if (dbBook == null)
        {
            throw new ArgumentNullException(nameof(dbBook));
        }
        _context.Entry(dbBook).CurrentValues.SetValues(book);
        await _context.SaveChangesAsync();
        return dbBook;
    }
}
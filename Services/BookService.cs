using LibraryApi.Models;
using LibraryApi.Entities;
using LibraryApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace LibraryApi.Services;

public class BookService : GenericCRUDService<Book>, IBookService
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly IMapper _mapper;

    public BookService(IDbContextFactory<DatabaseContext> contextFactory, IMapper mapper) : base(contextFactory)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task<PagedList<BookDTO_NestedAuthor>> GetBooks(int page, int pageSize, string? searchParameter, string searchProperty = "Genre")
    {
        var books = await base.GetItems(page, pageSize, searchParameter, searchProperty, book => book.Author!);

        var booksToRemove = new List<Book>();

        foreach (var book in books.Results!)
        {
            if (book.IsHidden)
            {
                booksToRemove.Add(book);
            }
        }

        foreach (var book in booksToRemove)
        {
            books.Results.Remove(book);
        }

        var mappedBooks = books.Results!.Select(_mapper.Map<BookDTO_NestedAuthor>).ToList();

        return new PagedList<BookDTO_NestedAuthor>
        {
            Count = books.Count,
            Results = mappedBooks,
            Next = books.Next
        };
    }

    public async Task<BookDTO_NestedAuthor> GetBook(int id)
    {
        var returnBook = await base.GetItem(id, book => book.Author!);
        if (returnBook.IsHidden)
        {
            throw new ArgumentException($"Book with id '{returnBook.Id}' is hidden");
        }
        return _mapper.Map<BookDTO_NestedAuthor>(returnBook);
    }

    public async Task<BookDTO_NestedAuthor> GetBook(string searchParameter, string searchProperty = "Title")
    {
        var returnBook = await base.GetItem(searchParameter, searchProperty, book => book.Author!);
        if (returnBook.IsHidden)
        {
            throw new ArgumentException($"Book with id '{returnBook.Id}' is hidden");
        }
        return _mapper.Map<BookDTO_NestedAuthor>(returnBook);
    }

    public async Task<BookDTO_NestedAuthor> AddBookGeneric(Book book)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        if (await context.Authors.FindAsync(book.AuthorId) == null)
        {
            throw new ArgumentException($"Author with id '{book.AuthorId}' does not exist");
        }
        var returnBook = await base.AddItem(book, "Title", book => book.Author!);
        return _mapper.Map<BookDTO_NestedAuthor>(returnBook);
    }

    public async Task<BookDTO_NestedAuthor> AddBookFramework(Book book)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        if (await context.Authors.FindAsync(book.AuthorId) == null)
        {
            throw new ArgumentException($"Author with id '{book.AuthorId}' does not exist");
        }

        var existingBook = await context.Books
            .Where(b => b.Title == book.Title && b.AuthorId == book.AuthorId)
            .FirstOrDefaultAsync();

        if (existingBook != null)
        {
            throw new ArgumentException($"Book with title '{book.Title}' and author id '{book.AuthorId}' already exists");
        }

        var returnBook = await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
        returnBook.Entity.Author = null;

        return _mapper.Map<BookDTO_NestedAuthor>(returnBook.Entity);
    }

    public async Task<BookDTO_NestedAuthor> AddBookSQL(Book book)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        await using var transaction = context.Database.BeginTransaction();

        var existingBook = await context.Books
            .FromSqlRaw("SELECT * FROM Books WHERE Title = @Title AND AuthorId = @AuthorId",
                new MySqlConnector.MySqlParameter("@Title", book.Title),
                new MySqlConnector.MySqlParameter("@AuthorId", book.AuthorId))
            .FirstOrDefaultAsync();

        if (existingBook != null)
        {
            throw new ArgumentException($"Book with title '{book.Title}' and author id '{book.AuthorId}' already exists");
        }

        if (await context.Authors
            .FromSqlRaw("SELECT * FROM Authors WHERE Id = @AuthorId",
                new MySqlConnector.MySqlParameter("@AuthorId", book.AuthorId))
            .FirstOrDefaultAsync() == null)
        {
            throw new ArgumentException($"Author with id '{book.AuthorId}' does not exist");
        }

        try
        {
            var sql = "INSERT INTO Books (Title, AuthorId, Genre, Year, imageUrl) VALUES (@Title, @AuthorId, @Genre, @Year, @imageUrl)";
            var parameters = new[]
            {
            new MySqlConnector.MySqlParameter("@Title", book.Title),
            new MySqlConnector.MySqlParameter("@AuthorId", book.AuthorId),
            new MySqlConnector.MySqlParameter("@Genre", book.Genre),
            new MySqlConnector.MySqlParameter("@Year", book.Year),
            new MySqlConnector.MySqlParameter("@imageUrl", book.ImageUrl)
        };
            await context.Database.ExecuteSqlRawAsync(sql, parameters);

            var returnBook = await context.Books
                .FromSqlRaw("SELECT * FROM Books WHERE Title = @Title AND AuthorId = @AuthorId",
                    new MySqlConnector.MySqlParameter("@Title", book.Title),
                    new MySqlConnector.MySqlParameter("@AuthorId", book.AuthorId))
                .FirstOrDefaultAsync();
            returnBook!.Author = null;
            var returnBookDTO = _mapper.Map<BookDTO_NestedAuthor>(returnBook);

            transaction.Commit();

            return returnBookDTO;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Failed to add book. Error: {ex.Message}");
        }
    }

    public async Task<BookDTO_NestedAuthor> UpdateBook(Book book)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        if (await context.Authors.FindAsync(book.AuthorId) == null)
        {
            throw new ArgumentException($"Author with id '{book.AuthorId}' does not exist");
        }
        var returnBook = await base.UpdateItem(book, "Title", book => book.Author!);
        return _mapper.Map<BookDTO_NestedAuthor>(returnBook);
    }

    public async Task<BookDTO_NestedAuthor> DeleteBook(int id)
    {
        var returnBook = await base.DeleteItem(id, book => book.Author!);
        return _mapper.Map<BookDTO_NestedAuthor>(returnBook);
    }
}
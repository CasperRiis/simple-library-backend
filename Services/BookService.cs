using LibraryApi.Models;
using LibraryApi.Entities;
using LibraryApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace LibraryApi.Managers;

public class BookService : GenericService<Book>, IBookService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public BookService(DatabaseContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedList<BookDTO_NestedAuthor>> GetBooks(int page, int pageSize, string? searchParameter, string searchProperty = "Genre")
    {
        var books = await base.GetItems(page, pageSize, searchParameter, searchProperty, book => book.Author!);

        var mappedBooks = books.Results!.Select(book => _mapper.Map<BookDTO_NestedAuthor>(book)).ToList();

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
        return _mapper.Map<BookDTO_NestedAuthor>(returnBook);
    }

    public async Task<BookDTO_NestedAuthor> GetBook(string searchParameter, string searchProperty = "Title")
    {
        var returnBook = await base.GetItem(searchParameter, searchProperty, book => book.Author!);
        return _mapper.Map<BookDTO_NestedAuthor>(returnBook);
    }

    public async Task<BookDTO_NestedAuthor> AddBook(Book book)
    {
        if (await _context.Authors.FindAsync(book.AuthorId) == null)
        {
            throw new ArgumentException($"Author with id '{book.AuthorId}' does not exist");
        }
        var returnBook = await base.AddItem(book, "Title", book => book.Author!);
        return _mapper.Map<BookDTO_NestedAuthor>(returnBook);
    }

    public async Task<BookDTO_NestedAuthor> UpdateBook(Book book)
    {
        if (await _context.Authors.FindAsync(book.AuthorId) == null)
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
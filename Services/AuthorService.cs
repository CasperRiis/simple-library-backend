
using LibraryApi.Models;
using LibraryApi.Entities;
using LibraryApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace LibraryApi.Managers;

public class AuthorService : GenericService<Author>, IAuthorService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public AuthorService(DatabaseContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedList<Author>> GetAuthors(int page, int pageSize, string? searchParameter, string searchProperty = "Name")
    {
        return await base.GetItems(page, pageSize, searchParameter, searchProperty);
    }

    public async Task<AuthorDTO_NestedBooks> GetAuthor(int id)
    {
        var author = await base.GetItem(id, author => author.Books);
        return _mapper.Map<AuthorDTO_NestedBooks>(author);
    }

    public async Task<Author> GetAuthor(string authorName)
    {
        return await base.GetItem(authorName, "Name");
    }

    public async Task<Author> AddAuthor(Author author)
    {
        return await base.AddItem(author, "Name");
    }

    public async Task<Author> DeleteAuthor(int authorId)
    {
        return await base.DeleteItem(authorId);
    }

    public async Task<Author> UpdateAuthor(Author author)
    {
        return await base.UpdateItem(author, "Name");
    }
}
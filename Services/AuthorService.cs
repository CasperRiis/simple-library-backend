
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

    public async Task<PagedList<AuthorDTO_NestedBooks>> GetAuthors(int page, int pageSize, string? searchParameter, string searchProperty = "Name")
    {
        var authors = await base.GetItems(page, pageSize, searchParameter, searchProperty, author => author.Books); ;

        var mappedAuthors = authors.Results!.Select(author => _mapper.Map<AuthorDTO_NestedBooks>(author)).ToList();

        return new PagedList<AuthorDTO_NestedBooks>
        {
            Count = authors.Count,
            Results = mappedAuthors,
            Next = authors.Next
        };
    }

    public async Task<AuthorDTO_NestedBooks> GetAuthor(int id)
    {
        var returnAuthor = await base.GetItem(id, author => author.Books);
        return _mapper.Map<AuthorDTO_NestedBooks>(returnAuthor);
    }

    public async Task<AuthorDTO_NestedBooks> GetAuthor(string searchParameter, string searchProperty = "Name")
    {
        var returnAuthor = await base.GetItem(searchParameter, searchProperty, author => author.Books);
        return _mapper.Map<AuthorDTO_NestedBooks>(returnAuthor);
    }

    public async Task<AuthorDTO_NestedBooks> AddAuthor(Author author)
    {
        var returnAuthor = await base.AddItem(author, "Name", author => author.Books);
        return _mapper.Map<AuthorDTO_NestedBooks>(returnAuthor);
    }

    public async Task<AuthorDTO_NestedBooks> UpdateAuthor(Author author)
    {
        var returnAuthor = await base.UpdateItem(author, "Name", author => author.Books);
        return _mapper.Map<AuthorDTO_NestedBooks>(returnAuthor);
    }

    public async Task<AuthorDTO_NestedBooks> DeleteAuthor(int authorId)
    {
        var returnAuthor = await base.DeleteItem(authorId, author => author.Books);
        return _mapper.Map<AuthorDTO_NestedBooks>(returnAuthor);
    }
}
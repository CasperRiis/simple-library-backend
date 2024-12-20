using AutoMapper;
using LibraryApi.Entities;
using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;
    private readonly IMapper _mapper;

    public AuthorController(IAuthorService authorService, IMapper mapper)
    {
        _authorService = authorService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<AuthorDTO_NestedBooks>> GetAuthors([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchParameter = null, [FromQuery] string searchProperty = "Name")
    {
        try
        {
            var authors = await _authorService.GetAuthors(page, pageSize, searchParameter, searchProperty);
            return Ok(authors);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDTO_NestedBooks>> GetAuthor(int id)
    {
        try
        {
            var author = await _authorService.GetAuthor(id);
            return Ok(author);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
            {
                return NotFound($"Author with id {id} not found");
            }
            return BadRequest(e.Message);
        }
    }

    [HttpGet("search/{searchParameter}")]
    public async Task<ActionResult<AuthorDTO_NestedBooks>> GetAuthor(string searchParameter, [FromQuery] string searchProperty = "Name")
    {
        try
        {
            var author = await _authorService.GetAuthor(searchParameter);
            return Ok(author);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
            {
                return NotFound($"Author with name {searchParameter} not found");
            }
            return BadRequest(e.Message);
        }
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<AuthorDTO_NestedBooks>> AddAuthor(AuthorDTO authorDTO)
    {
        var author = _mapper.Map<Author>(authorDTO);
        try
        {
            var createdAuthor = await _authorService.AddAuthor(author);
            string host = HttpContext.Request.Host.Value;
            string uri = $"{Request.Path}/{createdAuthor.Id}";
            return Created(uri, createdAuthor);
        }
        catch (Exception e)
        {
            if (e.Message.Contains($"Author with name {author.Name} already exists"))
            {
                return Conflict(e.Message);
            }
            return BadRequest(e.Message);
        }
    }

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<AuthorDTO_NestedBooks>> UpdateAuthor([FromBody] AuthorDTO authorDTO)
    {
        var author = _mapper.Map<Author>(authorDTO);
        try
        {
            var updatedAuthor = await _authorService.UpdateAuthor(author);
            return Ok(updatedAuthor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<AuthorDTO_NestedBooks>> DeleteAuthor(int id)
    {
        try
        {
            var author = await _authorService.DeleteAuthor(id);
            return Ok(author);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

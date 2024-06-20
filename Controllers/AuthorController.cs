using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<ActionResult<ArrayResponse<Author>>> GetAuthors([FromQuery] int startId)
    {
        try
        {
            var authors = await _authorService.GetAuthors();
            var results = authors.Skip(startId).Take(10);
            var count = authors.Count();
            var next = startId + 10;

            return Ok(new ArrayResponse<Author> { Count = count, Next = next, Results = results });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id)
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

    [HttpGet("name/{authorName}")]
    public async Task<ActionResult<Author>> GetAuthor(string authorName)
    {
        try
        {
            var author = await _authorService.GetAuthor(authorName);
            return Ok(author);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
            {
                return NotFound($"Author with name {authorName} not found");
            }
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Author>> AddAuthor(Author author)
    {
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

    [HttpPut]
    public async Task<ActionResult<Book>> UpdateAuthor([FromBody] Author author)
    {
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
        try
        {
            await _authorService.DeleteAuthor(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

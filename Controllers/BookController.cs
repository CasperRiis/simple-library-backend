using AutoMapper;
using LibraryApi.Entities;
using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;

    public BookController(IBookService bookService, IMapper mapper)
    {
        _bookService = bookService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<BookDTO_NestedAuthor>> GetBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchParameter = null, [FromQuery] string searchProperty = "Genre")
    {
        try
        {
            var books = await _bookService.GetBooks(page, pageSize, searchParameter, searchProperty);

            // Return un-altered object if user is an Admin
            if (IsUserRole("Admin"))
            {
                return Ok(books);
            }

            if (books.Results != null)
            {
                books.Results = books.Results.Where(b => !b.IsHidden).ToList();
            }

            return Ok(books);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDTO_NestedAuthor>> GetBook(int id)
    {
        try
        {
            var book = await _bookService.GetBook(id);

            if (IsUserRole("Admin"))
            {
                return Ok(book);
            }

            if (book.IsHidden)
            {
                return NotFound($"Book with id '{book.Id}' is hidden");
            }

            return Ok(book);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
            {
                return NotFound($"Book with id {id} not found");
            }
            return BadRequest(e.Message);
        }
    }

    [HttpGet("search/{searchParameter}")]
    public async Task<ActionResult<BookDTO_NestedAuthor>> GetBook(string searchParameter, [FromQuery] string searchProperty = "Title")
    {
        try
        {
            var book = await _bookService.GetBook(searchParameter, searchProperty);

            if (IsUserRole("Admin"))
            {
                return Ok(book);
            }

            if (book.IsHidden)
            {
                return NotFound($"Book with id '{book.Id}' is hidden");
            }

            return Ok(book);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
            {
                return NotFound($"Book with title {searchParameter} not found");
            }
            return BadRequest(e.Message);
        }
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookDTO_NestedAuthor>> AddBook(BookDTO bookDTO)
    {
        var book = _mapper.Map<Book>(bookDTO);
        try
        {
            var createdBook = await _bookService.AddBookGeneric(book);
            string host = HttpContext.Request.Host.Value;
            string uri = $"{Request.Path}/{createdBook.Id}";
            return Created(uri, createdBook);
        }
        catch (Exception e)
        {
            if (e.Message.Contains($"Book with title {book.Title} already exists"))
            {
                return Conflict(e.Message);
            }
            return BadRequest(e.Message);
        }
    }

    [HttpPost("Framework"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookDTO_NestedAuthor>> AddBookFramework(BookDTO bookDTO)
    {
        var book = _mapper.Map<Book>(bookDTO);
        try
        {
            var createdBook = await _bookService.AddBookFramework(book);
            string host = HttpContext.Request.Host.Value;
            string uri = $"{Request.Path}/{createdBook.Id}";
            return Created(uri, createdBook);
        }
        catch (Exception e)
        {
            if (e.Message.Contains($"already exists"))
            {
                return Conflict(e.Message);
            }
            return BadRequest(e.Message);
        }
    }

    [HttpPost("SQL"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookDTO_NestedAuthor>> AddBookSQL(BookDTO bookDTO)
    {
        var book = _mapper.Map<Book>(bookDTO);
        try
        {
            var createdBook = await _bookService.AddBookSQL(book);
            string host = HttpContext.Request.Host.Value;
            string uri = $"{Request.Path}/{createdBook.Id}";
            return Created(uri, createdBook);
        }
        catch (Exception e)
        {
            if (e.Message.Contains($"already exists"))
            {
                return Conflict(e.Message);
            }
            return BadRequest(e.Message);
        }
    }

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookDTO_NestedAuthor>> UpdateBook([FromBody] BookDTO bookDTO)
    {
        var book = _mapper.Map<Book>(bookDTO);
        try
        {
            // We don't need to check if the book is hidden here because it's only available to Admins
            var updatedBook = await _bookService.UpdateBook(book);
            return Ok(updatedBook);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookDTO_NestedAuthor>> DeleteBook(int id)
    {
        try
        {
            var book = await _bookService.DeleteBook(id);
            return Ok(book);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private bool IsUserRole(string role)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var jwtToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().ReadJwtToken(token);
            var userRoles = jwtToken.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

            if (userRoles.Contains(role))
            {
                return true;
            }
        }
        return false;
    }
}

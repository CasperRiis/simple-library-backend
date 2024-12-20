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

    [HttpGet("title/{bookTitle}")]
    public async Task<ActionResult<BookDTO_NestedAuthor>> GetBook(string bookTitle)
    {
        try
        {
            var book = await _bookService.GetBook(bookTitle);
            return Ok(book);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
            {
                return NotFound($"Book with title {bookTitle} not found");
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
            var createdBook = await _bookService.AddBook(book);
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

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<Book>> UpdateBook([FromBody] BookDTO bookDTO)
    {
        var book = _mapper.Map<Book>(bookDTO);
        try
        {
            var updatedBook = await _bookService.UpdateBook(book);
            return Ok(updatedBook);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookDTO>> DeleteBook(int id)
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
}

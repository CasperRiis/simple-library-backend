using Microsoft.AspNetCore.Mvc;
using LibraryApi.Entities;
using LibraryApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using LibraryApi.Models;
using AutoMapper;
using LibraryApi.RequestModels;

namespace FadeFactory_Account.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;
    private readonly IMapper _mapper;

    public AccountController(IAccountService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("{Id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<Account>> GetAccount(int Id)
    {
        Account account = await _service.GetAccount(Id);
        if (account.Id == -1) return NotFound($"No account with ID '{Id}'");
        return Ok(account);
    }

    [HttpGet(), Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
    {
        try
        {
            var accounts = await _service.GetAccounts();
            return Ok(accounts);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<Account>> CreateAccount(AccountRequest request)
    {
        var account = _mapper.Map<AccountDTO>(request);

        try
        {
            var createdAccount = await _service.CreateAccount(account);
            string host = HttpContext.Request.Host.Value;
            string uri = $"https://{host}/api/Accounts/{createdAccount.Id}";
            return Created(uri, createdAccount);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Account with email already exists."))
            {
                return Conflict(new { message = "Email is already in use" });
            }

            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{Id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAccount(int Id)
    {
        try
        {
            await _service.DeleteAccount(Id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut(), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAccount([FromBody] AccountDTO account)
    {
        try
        {
            Account updatedAccount = await _service.UpdateAccount(account);
            return Ok(updatedAccount);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(AccountRequest request)
    {
        var account = _mapper.Map<AccountDTO>(request);

        try
        {
            string token = await _service.Login(account);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

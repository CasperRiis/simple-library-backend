using Microsoft.AspNetCore.Mvc;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace FadeFactory_Accounts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountsController(IAccountService service)
    {
        _service = service;
    }

    [HttpGet("{AccountId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<Account>> GetAccount(int AccountId)
    {
        Account account = await _service.GetAccount(AccountId);
        if (account.AccountId == -1) return NotFound($"No account with ID '{AccountId}'");
        return Ok(account);
    }

    [HttpGet("getAll"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
    {
        try
        {
            var accounts = await _service.GetAllAccounts();
            return Ok(accounts);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<Account>> CreateAccount(AccountDTO account)
    {
        try
        {
            var createdAccount = await _service.CreateAccount(account);
            string host = HttpContext.Request.Host.Value;
            string uri = $"https://{host}/api/Accounts/{createdAccount.AccountId}";
            return Created(uri, createdAccount);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Account with username already exists."))
            {
                return Conflict(new { message = "Username is already in use" });
            }

            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{AccountId}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAccount(int AccountId)
    {
        try
        {
            await _service.DeleteAccount(AccountId);
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
    public async Task<ActionResult<string>> Login(AccountDTO request)
    {
        try
        {
            string token = await _service.Login(request);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

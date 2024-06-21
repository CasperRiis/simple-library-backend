using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Helpers;

namespace LibraryApi.Managers;

public class AccountDbManager : IAccountService
{
    private readonly AuthHelper _authHelper;
    private readonly AccountDbContext _context;

    public AccountDbManager(AccountDbContext context, AuthHelper authHelper)
    {
        _authHelper = authHelper;
        _context = context;
    }

    public async Task<Account> CreateAccount(AccountDTO accountDTO)
    {
        if (accountDTO.Username == null || accountDTO.Username == null || accountDTO.Password == null)
            throw new Exception("A string is null.");

        int dbSize = (await _context.Accounts.ToListAsync()).LastOrDefault()?.AccountId ?? 0;
        accountDTO.AccountId = dbSize + 1;

        if (_context.Accounts.Count(a => a.Username == accountDTO.Username) > 0)
            throw new Exception("Account with username already exists.");

        accountDTO.IsAdmin = false;

        var account = accountDTO.Adapt();
        var result = _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task DeleteAccount(int AccountId)
    {
        _context.Accounts.Remove(new Account { AccountId = AccountId });
        await _context.SaveChangesAsync();
    }

    public async Task<Account> GetAccount(int AccountId)
    {
        var result = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == AccountId);
        if (result == null) throw new Exception("Account not found.");
        return result;
    }

    public async Task<IEnumerable<Account>> GetAccounts()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<Account> UpdateAccount(AccountDTO accountDTO)
    {
        var account = accountDTO.Adapt();
        if (_context.Accounts.Count(a => a.Username == account.Username && a.AccountId != account.AccountId) > 0)
        {
            throw new Exception("Account with this username already exists.");
        }

        var dbAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == account.AccountId);
        if (dbAccount != null)
        {
            if (account.Username != null)
            {
                dbAccount.Username = account.Username;
            }
            if (account.PasswordHash != null)
            {
                dbAccount.PasswordHash = account.PasswordHash;
            }
            if (account.PasswordSalt != null)
            {
                dbAccount.PasswordSalt = account.PasswordSalt;
            }
            dbAccount.IsAdmin = account.IsAdmin;

            await _context.SaveChangesAsync();
            return dbAccount;
        }

        throw new Exception("Account not found.");
    }

    public async Task<string> Login(AccountDTO loginRequest)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == loginRequest.Username) ?? throw new Exception("Invalid credentials.");
        if (!_authHelper.VerifyPasswordHash(loginRequest.Password!, account.PasswordHash!, account.PasswordSalt!))
        {
            throw new Exception("Invalid credentials.");
        }
        return _authHelper.CreateToken(account);
    }
}

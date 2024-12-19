using LibraryApi.Entities;
using LibraryApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Helpers;
using LibraryApi.Models;

namespace LibraryApi.Managers;

public class AccountService : GenericService<Account>, IAccountService
{
    private readonly AuthHelper _authHelper;
    private readonly DatabaseContext _context;

    public AccountService(DatabaseContext context, AuthHelper authHelper) : base(context)
    {
        _authHelper = authHelper;
        _context = context;
    }

    public async Task<IEnumerable<Account>> GetAccounts()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<Account> GetAccount(int AccountId)
    {
        return await base.GetItem(AccountId);
    }

    public async Task<Account> CreateAccount(AccountDTO accountDTO)
    {
        if (string.IsNullOrEmpty(accountDTO.Username))
            throw new ArgumentException("Username is null or empty.", nameof(accountDTO.Username));

        if (string.IsNullOrEmpty(accountDTO.Password))
            throw new ArgumentException("Password is null or empty.", nameof(accountDTO.Password));

        if (_context.Accounts.Count(a => a.Username == accountDTO.Username) > 0)
            throw new Exception("Account with username already exists.");

        //Ensure that no user created through API is admin
        accountDTO.IsAdmin = false;

        var account = accountDTO.Adapt();
        var result = _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<Account> UpdateAccount(AccountDTO accountDTO)
    {
        var account = accountDTO.Adapt();
        if (_context.Accounts.Count(a => a.Username == account.Username && a.Id != account.Id) > 0)
        {
            throw new Exception("Account with this username already exists.");
        }

        var dbAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id);
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

    public async Task<Account> DeleteAccount(int AccountId)
    {
        return await base.DeleteItem(AccountId);
    }
}

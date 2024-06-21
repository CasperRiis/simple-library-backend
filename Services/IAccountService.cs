using LibraryApi.Models;

namespace LibraryApi.Services;
public interface IAccountService
{
    Task<Account> GetAccount(int AccountId);
    Task<IEnumerable<Account>> GetAccounts();
    Task<Account> CreateAccount(AccountDTO account);
    Task<Account> UpdateAccount(AccountDTO account);
    Task DeleteAccount(int AccountId);
    Task<string> Login(AccountDTO loginRequest);
}


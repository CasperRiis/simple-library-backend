using LibraryApi.Entities;
using LibraryApi.Models;

namespace LibraryApi.Interfaces;
public interface IAccountService
{
    Task<Account> GetAccount(int AccountId);
    Task<IEnumerable<Account>> GetAccounts();
    Task<Account> CreateAccount(AccountDTO account);
    Task<Account> UpdateAccount(AccountDTO account);
    Task<Account> DeleteAccount(int AccountId);
    Task<string> Login(AccountDTO loginRequest);
}


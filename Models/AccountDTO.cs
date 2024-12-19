using LibraryApi.Helpers;
using LibraryApi.Entities;

namespace LibraryApi.Models;

public class AccountDTO : BaseAccount
{
    private readonly AuthHelper _authHelper = AuthHelper.Instance;

    public string? Password { get; set; }

    public Account Adapt()
    {
        byte[]? passwordHash, passwordSalt;
        if (!string.IsNullOrEmpty(Password))
        {
            _authHelper.CreatePasswordHash(Password, out passwordHash, out passwordSalt);
        }
        else
        {
            throw new Exception("Password is null.");
        }

        return new Account
        {
            Id = Id,
            Username = Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            IsAdmin = IsAdmin
        };
    }
}


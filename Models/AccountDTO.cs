using LibraryApi.Helpers;
using Newtonsoft.Json;

namespace LibraryApi.Models;

public class AccountDTO : AccountParent
{
    private readonly AuthHelper _authHelper = AuthHelper.Instance;

    [JsonProperty("password")]
    public string? Password { get; set; }

    public Account Adapt()
    {
        byte[]? passwordHash, passwordSalt;
        if (Password != null)
        {
            _authHelper.CreatePasswordHash(Password, out passwordHash, out passwordSalt);
        }
        else
        {
            passwordHash = null;
            passwordSalt = null;
        }

        return new Account
        {
            AccountId = AccountId,
            Username = Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            IsAdmin = IsAdmin
        };
    }
}


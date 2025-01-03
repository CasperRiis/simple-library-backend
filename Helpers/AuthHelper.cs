using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LibraryApi.Entities;
using Microsoft.IdentityModel.Tokens;

namespace LibraryApi.Helpers;

public class AuthHelper
{
    private static string _jwtTokenSecret = "";

    public AuthHelper(string jwtTokenSecret)
    {
        _jwtTokenSecret = jwtTokenSecret;
    }

    private static AuthHelper? _instance;

    private AuthHelper()
    {
    }

    public static AuthHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AuthHelper();
            }
            return _instance;
        }
    }

    public string CreateToken(Account account)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Email!.ToString()),
                new Claim(ClaimTypes.Role, account.IsAdmin ? "Admin" : "User")
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.Now.AddDays(10),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
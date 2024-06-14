using Microsoft.AspNetCore.Authentication;

namespace LibraryApi.Middleware;

public class KeyAuthSchemeOptions : AuthenticationSchemeOptions
{
    public string? ApiKey { get; set; }
}
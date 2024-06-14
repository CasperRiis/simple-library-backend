using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LibraryApi.Middleware;

public class KeyAuthSchemeHandler : AuthenticationHandler<KeyAuthSchemeOptions>
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public KeyAuthSchemeHandler(
        IOptionsMonitor<KeyAuthSchemeOptions> options,
        ILoggerFactory logger,
        IConfiguration config,
        IWebHostEnvironment env,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
        _config = config;
        _env = env;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? validApiKey;
        if (_env.IsDevelopment()) //pull secrets from local storage or Azure configuration
        {
            validApiKey = _config["validApiKey"] ?? throw new ArgumentNullException();
        }
        else
        {
            validApiKey = Environment.GetEnvironmentVariable("validApiKey") ?? throw new ArgumentNullException();
        }
        var apiKey = Context.Request.Headers["X-API-KEY"];
        if (apiKey != validApiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid X-API-KEY"));
        }
        var claims = new[] { new Claim(ClaimTypes.Name, "VALID USER") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

using System.Security.Claims;
using System.Text;
using LibraryApi.Helpers;
using LibraryApi.Managers;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string cosmosConnectionString;
string cosmosDbName;
string jwtTokenSecret;

if (builder.Environment.IsDevelopment()) //pull secrets from local storage or Azure configuration
{
    cosmosConnectionString = builder.Configuration["cosmosConnectionString"] ?? throw new ArgumentNullException();
    cosmosDbName = builder.Configuration["cosmosDbName"] ?? throw new ArgumentNullException();
    jwtTokenSecret = builder.Configuration["jwtTokenSecret"] ?? throw new ArgumentNullException();
}
else
{
    cosmosConnectionString = Environment.GetEnvironmentVariable("cosmosConnectionString") ?? throw new ArgumentNullException();
    cosmosDbName = Environment.GetEnvironmentVariable("cosmosDbName") ?? throw new ArgumentNullException();
    jwtTokenSecret = Environment.GetEnvironmentVariable("jwtTokenSecret") ?? throw new ArgumentNullException();
}

builder.Services.AddDbContext<BookDbContext>(options => options.UseCosmos(cosmosConnectionString, cosmosDbName));
builder.Services.AddScoped<IBookService, BookDbManager>();

builder.Services.AddDbContext<AuthorDbContext>(options => options.UseCosmos(cosmosConnectionString, cosmosDbName));
builder.Services.AddScoped<IAuthorService, AuthorDbManager>();

builder.Services.AddDbContext<AccountDbContext>(options => options.UseCosmos(cosmosConnectionString, cosmosDbName));
builder.Services.AddScoped<IAccountService, AccountDbManager>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Bearer { TOKEN }",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = ClaimTypes.Role,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddScoped(sp => new AuthHelper(jwtTokenSecret));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<BookDbContext>().Database.EnsureCreated();
    scope.ServiceProvider.GetRequiredService<AuthorDbContext>().Database.EnsureCreated();
    scope.ServiceProvider.GetRequiredService<AccountDbContext>().Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
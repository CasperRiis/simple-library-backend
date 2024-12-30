using System.Security.Claims;
using System.Text;
using LibraryApi.Helpers;
using LibraryApi.Services;
using LibraryApi.Entities;
using LibraryApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program)); //AutoMapper configuration
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionStrings = new List<string>();
string jwtTokenSecret;

if (builder.Environment.IsDevelopment()) //pull secrets from local storage or Azure configuration
{
    connectionStrings.Add(builder.Configuration.GetValue<string>("MYSQL_CONNECTION_STRING") ?? throw new ArgumentNullException());
    connectionStrings.Add(builder.Configuration.GetValue<string>("MYSQL_CONNECTION_STRING_DOCKER") ?? throw new ArgumentNullException());
    jwtTokenSecret = builder.Configuration.GetValue<string>("JWT_TOKEN_SECRET") ?? throw new ArgumentNullException();
}
else
{
    connectionStrings.Add(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING") ?? throw new ArgumentNullException());
    jwtTokenSecret = Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET") ?? throw new ArgumentNullException();
}

string? connectionString = null;
bool connected = false;

foreach (var connStr in connectionStrings)
{
    try
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));

        using (var context = new DatabaseContext(optionsBuilder.Options))
        {
            if (context.Database.CanConnect())
            {
                connectionString = connStr;
                connected = true;
                break;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to connect using connection string: {connStr}. Error: {ex.Message}");
    }
}

if (!connected)
{
    throw new Exception("Failed to connect to any database.");
}

builder.Services.AddDbContextFactory<DatabaseContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();

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
{ //TODO Fix this
    options.AddDefaultPolicy(
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    var retryCount = 5;
    var delay = TimeSpan.FromSeconds(10);

    for (int i = 0; i < retryCount; i++)
    {
        try
        {
            dbContext.Database.Migrate(); //Auto perform migrations
            DbSeeder.UpsertSeed(dbContext); //Auto seed database
            break;
        }
        catch (SqlException)
        {
            if (i == retryCount - 1) throw;
            Thread.Sleep(delay);
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lib-Mgmt-API Development Swagger");
        c.RoutePrefix = string.Empty;
        c.DocumentTitle = "Lib-Mgmt-API Development Swagger";
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lib-Mgmt-API Production Swagger");
        c.RoutePrefix = string.Empty;
        c.DocumentTitle = "Lib-Mgmt-API Swagger";
    });
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
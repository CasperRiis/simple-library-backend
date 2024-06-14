using LibraryApi.Managers;
using LibraryApi.Middleware;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string cosmosConnectionString;
string cosmosDbName;

if (builder.Environment.IsDevelopment()) //pull secrets from local storage or Azure configuration
{
    cosmosConnectionString = builder.Configuration["cosmosConnectionString"] ?? throw new ArgumentNullException();
    cosmosDbName = builder.Configuration["cosmosDbName"] ?? throw new ArgumentNullException();
}
else
{
    cosmosConnectionString = Environment.GetEnvironmentVariable("cosmosConnectionString") ?? throw new ArgumentNullException();
    cosmosDbName = Environment.GetEnvironmentVariable("cosmosDbName") ?? throw new ArgumentNullException();
}

builder.Services.AddDbContext<BookDbContext>(options => options.UseCosmos(cosmosConnectionString, cosmosDbName));
builder.Services.AddScoped<IBookService, BookDbManager>();

builder.Services.AddDbContext<AuthorDbContext>(options => options.UseCosmos(cosmosConnectionString, cosmosDbName));
builder.Services.AddScoped<IAuthorService, AuthorDbManager>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "x-api-key",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication()
    .AddScheme<KeyAuthSchemeOptions, KeyAuthSchemeHandler>("ApiKey", options => { });


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
    var dbContext = scope.ServiceProvider.GetRequiredService<BookDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
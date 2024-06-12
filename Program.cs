using LibraryApi.Managers;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;

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

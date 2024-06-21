using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Models;

public class AuthorDbContext : DbContext
{
    public AuthorDbContext(DbContextOptions<AuthorDbContext> options) : base(options) { }

    public virtual DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultContainer("Library");
        builder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Nationality).IsRequired().HasMaxLength(100);
            entity.Property(e => e.BirthYear).IsRequired();
        });
    }
}
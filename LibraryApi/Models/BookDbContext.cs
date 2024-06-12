using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Models;

public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultContainer("Library");
        builder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.AuthorId).IsRequired();
            entity.Property(e => e.Genre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Year).IsRequired();
        });
    }
}
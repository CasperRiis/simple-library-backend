using LibraryApi.Entities;
using LibraryApi.Models;

public static class DbSeeder
{
    private static string randomBookImageUrl // Random image URL for seeding until blob storage is implemented
    {
        get
        {
            string[] urls = new string[] {
                "https://i.imgur.com/O0lr2Cd.jpg",
                "https://i.imgur.com/bMgtpA5.jpg",
                "https://i.imgur.com/1i5agqs.png",
                "https://i.imgur.com/4zoz7ob.jpg",
                "https://i.imgur.com/6eQFyss.jpg",
                "https://i.imgur.com/bC0oxse.png"
            };
            return urls[new Random().Next(0, urls.Length)];
        }
    }

    private static string randomAuthorImageUrl // Random image URL for seeding until blob storage is implemented
    {
        get
        {
            string[] urls = new string[] {
                "https://i.imgur.com/mo8DPjt.jpeg",
                "https://i.imgur.com/FeeulQD.jpeg",
                "https://i.imgur.com/HBqbH38.jpeg",
                "https://i.imgur.com/HqGl4f2.jpeg",
                "https://i.imgur.com/Sf9zNKv.jpeg",
                "https://i.imgur.com/EybvU87.jpeg"
            };
            return urls[new Random().Next(0, urls.Length)];
        }
    }

    public static void UpsertSeed(DatabaseContext context)
    {
        // Seed Accounts
        UpsertAccount(context, new AccountDTO { Id = 1, Username = "string", IsAdmin = true, Password = "string" }.Adapt());
        UpsertAccount(context, new AccountDTO { Id = 2, Username = "Admin", IsAdmin = true, Password = "Admin123" }.Adapt());
        UpsertAccount(context, new AccountDTO { Id = 3, Username = "User", IsAdmin = false, Password = "User123" }.Adapt());
        context.SaveChanges();

        // Seed Authors
        UpsertAuthor(context, new Author { Id = 1, Name = "Alex", Nationality = "Albanian", BirthYear = 1970, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 2, Name = "Bob", Nationality = "Bhutanese", BirthYear = 1980, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 3, Name = "Charlie", Nationality = "Czech", BirthYear = 1990, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 4, Name = "David", Nationality = "Danish", BirthYear = 2000, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 5, Name = "Edward", Nationality = "Estonian", BirthYear = 2010, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 6, Name = "Frank", Nationality = "Finnish", BirthYear = 2020, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 7, Name = "George", Nationality = "German", BirthYear = 2030, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 8, Name = "Harry", Nationality = "Hungarian", BirthYear = 2040, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 9, Name = "Ivan", Nationality = "Icelandic", BirthYear = 2050, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 10, Name = "Jack", Nationality = "Italian", BirthYear = 2060, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 11, Name = "Kevin", Nationality = "Kazakh", BirthYear = 2070, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 12, Name = "Liam", Nationality = "Latvian", BirthYear = 2080, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 13, Name = "Michael", Nationality = "Moldovan", BirthYear = 2090, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 14, Name = "Nathan", Nationality = "Norwegian", BirthYear = 2100, ImageUrl = randomAuthorImageUrl });
        UpsertAuthor(context, new Author { Id = 15, Name = "Oscar", Nationality = "Omani", BirthYear = 2110, ImageUrl = randomAuthorImageUrl });
        context.SaveChanges();

        // Seed Books
        var authorIds = context.Authors.Select(a => a.Id).ToArray();
        var random = new Random();
        var randomAuthorId = authorIds[random.Next(authorIds.Length)];
        UpsertBook(context, new Book { Id = 1, Title = "The Plot Against America", AuthorId = randomAuthorId, Genre = "Alternative Fiction", Year = 2000, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 2, Title = "American Pastoral", AuthorId = randomAuthorId, Genre = "Historical Fiction", Year = 2001, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 3, Title = "I Married a Communist", AuthorId = randomAuthorId, Genre = "Political Fiction", Year = 2002, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 4, Title = "The Double Helix", AuthorId = randomAuthorId, Genre = "Biography", Year = 2010, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 5, Title = "The Gene", AuthorId = randomAuthorId, Genre = "Science", Year = 2011, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 6, Title = "Genome", AuthorId = randomAuthorId, Genre = "Science", Year = 2012, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 7, Title = "Life of Pi", AuthorId = randomAuthorId, Genre = "Commercial Fiction", Year = 2020, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 8, Title = "Beatrice and Virgil", AuthorId = randomAuthorId, Genre = "Fiction", Year = 2021, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 9, Title = "The High Mountains of Portugal", AuthorId = randomAuthorId, Genre = "Fiction", Year = 2022, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 10, Title = "1984", AuthorId = randomAuthorId, Genre = "Dystopian", Year = 2030, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 11, Title = "Animal Farm", AuthorId = randomAuthorId, Genre = "Political Satire", Year = 2031, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 12, Title = "Homage to Catalonia", AuthorId = randomAuthorId, Genre = "Memoir", Year = 2032, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 13, Title = "Common Sense", AuthorId = randomAuthorId, Genre = "Essay", Year = 2040, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 14, Title = "The Rights of Man", AuthorId = randomAuthorId, Genre = "Political Philosophy", Year = 2041, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 15, Title = "The Age of Reason", AuthorId = randomAuthorId, Genre = "Philosophy", Year = 2042, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 16, Title = "Brave New World", AuthorId = randomAuthorId, Genre = "Dystopian", Year = 2033, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 17, Title = "Fahrenheit 451", AuthorId = randomAuthorId, Genre = "Dystopian", Year = 2034, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 18, Title = "The Handmaid's Tale", AuthorId = randomAuthorId, Genre = "Dystopian", Year = 2035, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 19, Title = "The Road", AuthorId = randomAuthorId, Genre = "Post-Apocalyptic", Year = 2036, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 20, Title = "The Catcher in the Rye", AuthorId = randomAuthorId, Genre = "Literary Fiction", Year = 2037, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 21, Title = "To Kill a Mockingbird", AuthorId = randomAuthorId, Genre = "Southern Gothic", Year = 2038, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 22, Title = "Pride and Prejudice", AuthorId = randomAuthorId, Genre = "Romance", Year = 2039, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 23, Title = "Moby-Dick", AuthorId = randomAuthorId, Genre = "Adventure", Year = 2043, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 24, Title = "War and Peace", AuthorId = randomAuthorId, Genre = "Historical Fiction", Year = 2044, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 25, Title = "Crime and Punishment", AuthorId = randomAuthorId, Genre = "Psychological Fiction", Year = 2045, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 26, Title = "The Brothers Karamazov", AuthorId = randomAuthorId, Genre = "Philosophical Fiction", Year = 2046, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 27, Title = "Great Expectations", AuthorId = randomAuthorId, Genre = "Literary Fiction", Year = 2047, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 28, Title = "Jane Eyre", AuthorId = randomAuthorId, Genre = "Gothic Fiction", Year = 2048, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 29, Title = "Wuthering Heights", AuthorId = randomAuthorId, Genre = "Gothic Fiction", Year = 2049, ImageUrl = randomBookImageUrl });
        UpsertBook(context, new Book { Id = 30, Title = "The Great Gatsby", AuthorId = randomAuthorId, Genre = "Tragedy", Year = 2050, ImageUrl = randomBookImageUrl });

        context.SaveChanges();
    }

    private static void UpsertAccount(DatabaseContext context, Account account)
    {
        var existingAccount = context.Accounts.Find(account.Id);
        if (existingAccount == null)
        {
            context.Accounts.Add(account);
        }
        else
        {
            context.Entry(existingAccount).CurrentValues.SetValues(account);
        }
    }

    private static void UpsertAuthor(DatabaseContext context, Author author)
    {
        var existingAuthor = context.Authors.Find(author.Id);
        if (existingAuthor == null)
        {
            context.Authors.Add(author);
        }
        else
        {
            context.Entry(existingAuthor).CurrentValues.SetValues(author);
        }
    }

    private static void UpsertBook(DatabaseContext context, Book book)
    {
        var existingBook = context.Books.Find(book.Id);
        if (existingBook == null)
        {
            context.Books.Add(book);
        }
        else
        {
            context.Entry(existingBook).CurrentValues.SetValues(book);
        }
    }
}
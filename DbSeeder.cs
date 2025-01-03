using LibraryApi.Entities;
using LibraryApi.Models;
using LibraryApi.Interfaces;

public static class DbSeeder
{
    public static async Task UpsertSeedAsync(DatabaseContext context, IImageService imageService)
    {
        if (context.Accounts.Any())
        {
            return;
        }

        var random = new Random();
        var ImageUrlsBooksArray = await SeedBlobStorageImages(imageService, "_seed-images/books");
        var ImageUrlsAuthorsArray = await SeedBlobStorageImages(imageService, "_seed-images/authors");

        // Seed Accounts
        UpsertAccount(context, new AccountDTO { Id = 1, Email = "Admin@test.com", IsAdmin = true, Password = "Admin123" }.Adapt());
        UpsertAccount(context, new AccountDTO { Id = 2, Email = "User@test.com", IsAdmin = false, Password = "User123" }.Adapt());
        await context.SaveChangesAsync();

        // Seed Authors
        UpsertAuthor(context, new Author { Id = 1, Name = "Alex", Nationality = "Albanian", BirthYear = 1970, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 2, Name = "Bob", Nationality = "Bhutanese", BirthYear = 1980, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 3, Name = "Charlie", Nationality = "Czech", BirthYear = 1990, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 4, Name = "David", Nationality = "Danish", BirthYear = 2000, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 5, Name = "Edward", Nationality = "Estonian", BirthYear = 2010, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 6, Name = "Frank", Nationality = "Finnish", BirthYear = 2020, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 7, Name = "George", Nationality = "German", BirthYear = 2030, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 8, Name = "Harry", Nationality = "Hungarian", BirthYear = 2040, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 9, Name = "Ivan", Nationality = "Icelandic", BirthYear = 2050, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 10, Name = "Jack", Nationality = "Italian", BirthYear = 2060, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 11, Name = "Kevin", Nationality = "Kazakh", BirthYear = 2070, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 12, Name = "Liam", Nationality = "Latvian", BirthYear = 2080, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 13, Name = "Michael", Nationality = "Moldovan", BirthYear = 2090, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 14, Name = "Nathan", Nationality = "Norwegian", BirthYear = 2100, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        UpsertAuthor(context, new Author { Id = 15, Name = "Oscar", Nationality = "Omani", BirthYear = 2110, ImageUrl = ImageUrlsAuthorsArray[random.Next(ImageUrlsAuthorsArray.Length)] });
        await context.SaveChangesAsync();

        // Seed Books
        var authorIds = context.Authors.Select(a => a.Id).ToArray();
        UpsertBook(context, new Book { Id = 1, Title = "The Plot Against America", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Alternative Fiction", Year = 2000, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 2, Title = "American Pastoral", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Historical Fiction", Year = 2001, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 3, Title = "I Married a Communist", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Political Fiction", Year = 2002, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 4, Title = "The Double Helix", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Biography", Year = 2010, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 5, Title = "The Gene", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Science", Year = 2011, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 6, Title = "Genome", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Science", Year = 2012, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 7, Title = "Life of Pi", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Commercial Fiction", Year = 2020, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 8, Title = "Beatrice and Virgil", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Fiction", Year = 2021, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 9, Title = "The High Mountains of Portugal", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Fiction", Year = 2022, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 10, Title = "1984", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Dystopian", Year = 2030, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 11, Title = "Animal Farm", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Political Satire", Year = 2031, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 12, Title = "Homage to Catalonia", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Memoir", Year = 2032, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 13, Title = "Common Sense", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Essay", Year = 2040, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 14, Title = "The Rights of Man", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Political Philosophy", Year = 2041, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 15, Title = "The Age of Reason", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Philosophy", Year = 2042, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 16, Title = "Brave New World", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Dystopian", Year = 2033, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 17, Title = "Fahrenheit 451", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Dystopian", Year = 2034, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 18, Title = "The Handmaid's Tale", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Dystopian", Year = 2035, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 19, Title = "The Road", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Post-Apocalyptic", Year = 2036, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 20, Title = "The Catcher in the Rye", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Literary Fiction", Year = 2037, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 21, Title = "To Kill a Mockingbird", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Southern Gothic", Year = 2038, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 22, Title = "Pride and Prejudice", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Romance", Year = 2039, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 23, Title = "Moby-Dick", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Adventure", Year = 2043, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 24, Title = "War and Peace", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Historical Fiction", Year = 2044, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 25, Title = "Crime and Punishment", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Psychological Fiction", Year = 2045, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 26, Title = "The Brothers Karamazov", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Philosophical Fiction", Year = 2046, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 27, Title = "Great Expectations", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Literary Fiction", Year = 2047, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 28, Title = "Jane Eyre", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Gothic Fiction", Year = 2048, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 29, Title = "Wuthering Heights", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Gothic Fiction", Year = 2049, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        UpsertBook(context, new Book { Id = 30, Title = "The Great Gatsby", AuthorId = authorIds[random.Next(authorIds.Length)], Genre = "Tragedy", Year = 2050, ImageUrl = ImageUrlsBooksArray[random.Next(ImageUrlsBooksArray.Length)] });
        await context.SaveChangesAsync();
    }

    private static async Task<string[]> SeedBlobStorageImages(IImageService imageService, string seedImagesFolder)
    {
        var ImageUrls = new List<string>();
        string seedImagesFolderFull = Path.Combine(Directory.GetCurrentDirectory(), seedImagesFolder);
        if (Directory.Exists(seedImagesFolderFull))
        {
            foreach (var filePath in Directory.GetFiles(seedImagesFolderFull))
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    string fileName = Path.GetFileName(filePath);
                    string imageUrl = await imageService.UploadImageAsync(stream, fileName);
                    Console.WriteLine($"Uploaded {fileName} to {imageUrl}");
                    ImageUrls.Add(imageUrl);
                }
            }
        }
        else
        {
            Console.WriteLine($"Directory {seedImagesFolderFull} does not exist.");
        }
        return ImageUrls.ToArray();
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
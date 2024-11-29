using BookServer.Models;
using BookServer.Services;
using Shared.Enums;

namespace BookServer.Extensions;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Books.Any())
        {
            var now = DateTime.Now;
            var books = new[] {
                new BookModel { Id = Guid.NewGuid(), Genre = Genre.Fantasy, Title = "Title1", Author = "Author1", Created = now, Updated = now},
                new BookModel { Id = Guid.NewGuid(), Genre = Genre.Mystery, Title = "Title2", Author = "Author2", Created = now, Updated = now },
                new BookModel { Id = Guid.NewGuid(), Genre = Genre.Horror, Title = "Title3", Author = "Author3", Created = now, Updated = now },
                new BookModel { Id = Guid.NewGuid(), Genre = Genre.Adventure, Title = "Title4", Author = "Author4", Created = now, Updated = now },
                };
            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}

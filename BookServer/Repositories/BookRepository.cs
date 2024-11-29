using BookServer.Interfaces;
using BookServer.Models;
using BookServer.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace BookServer.Repositories;

public class BookRepository(AppDbContext dbContext) : IBookRepository
{
    public async Task<Guid> UpsertAsync(BookModel entity)
    {
        var existingEntity = await dbContext.Books
            .FirstOrDefaultAsync(f => f.Id == entity.Id);

        var now = DateTime.Now;
        entity.Updated = now;
        if (existingEntity == null)
        {
            entity.Created = now;
            await dbContext.Books.AddAsync(entity);
        }
        else
        {
            entity.Created = existingEntity.Created;
            dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        }

        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<IEnumerable<BookModel>> GetBooksAsync(BookFilter filter)
    {
        var query = dbContext.Books.AsQueryable();

        if (filter.Genre.HasValue)
            query = query.Where(task => task.Genre == filter.Genre);

        if (!string.IsNullOrEmpty(filter.Title))
            query = query.Where(task => task.Title.Contains(filter.Title));

        if (!string.IsNullOrEmpty(filter.Author))
            query = query.Where(task => task.Author.Contains(filter.Author));

        return await query.ToListAsync();
    }

    public async Task<BookModel?> GetBookByIdAsync(Guid id) =>
        await dbContext.Books.FirstOrDefaultAsync(f => f.Id == id);

    public async Task<bool> RemoveAsync(Guid id)
    {
        var book = await dbContext.Books.FindAsync(id);

        if (book != null)
        {
            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();
        }
        return true;
    }

    public async Task AddBooksAsync(List<BookModel> books)
    {
        var now = DateTime.Now;
        books.ForEach(f =>
        {
            f.Created = now;
            f.Updated = now;
        });
        await dbContext.Books.AddRangeAsync(books);
        await dbContext.SaveChangesAsync();
    }
}

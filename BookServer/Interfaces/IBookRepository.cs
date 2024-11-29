using BookServer.Models;
using Shared.Models;

namespace BookServer.Interfaces;

public interface IBookRepository
{
    Task<Guid> UpsertAsync(BookModel mapped);
    Task<IEnumerable<BookModel>> GetBooksAsync(BookFilter filter);
    Task<BookModel?> GetBookByIdAsync(Guid id);
    Task<bool> RemoveAsync(Guid id);
    Task AddBooksAsync(List<BookModel> records);
}

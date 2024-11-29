using BookServer.Models;
using Shared.Models;

namespace BookServer.Interfaces;

public interface IBookService
{
    Task<BookView> GetBookByIdAsync(Guid id);
    Task<IEnumerable<BookView>> GetBooksAsync(BookFilter filter);
    Task<bool> RemoveBookAsync(Guid id);
    Task<(bool success, string errorMsg)> UploadCsvAsync(IFormFile file);
    Task<Guid> UpsertBookAsync(UpsertBookArg filter);
}

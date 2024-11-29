using AutoMapper;
using BookServer.Extensions;
using BookServer.Interfaces;
using BookServer.Models;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.SignalR;
using Shared.Models;
using System.Globalization;

namespace BookServer.Services;

public class BookService(IBookRepository bookRepository, IMapper mapper, IHubContext<TableHub> hubContext)
    : IBookService
{
    public async Task<Guid> UpsertBookAsync(UpsertBookArg args)
    {
        var mapped = mapper.Map<BookModel>(args);
        Guid newId = await bookRepository.UpsertAsync(mapped);

        await hubContext.Clients.All.SendAsync("ReceiveTableUpdate");
        return newId;
    }

    public async Task<IEnumerable<BookView>> GetBooksAsync(BookFilter filter)
    {
        IEnumerable<BookModel> books = await bookRepository.GetBooksAsync(filter);
        var mapped = mapper.Map<List<BookView>>(books);
        return mapped;
    }

    public async Task<BookView> GetBookByIdAsync(Guid id)
    {
        BookModel? book = await bookRepository.GetBookByIdAsync(id);
        var mapped = mapper.Map<BookView>(book);
        return mapped;
    }

    public async Task<bool> RemoveBookAsync(Guid id)
    {
        bool success = await bookRepository.RemoveAsync(id);
        await hubContext.Clients.All.SendAsync("ReceiveTableUpdate");

        return success;
    }

    public async Task<(bool success, string errorMsg)> UploadCsvAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return (false, "No file uploaded.");

        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            var records = csv.GetRecords<CsvBookRow>().ToList();

            if (records.Count != 0)
            {
                var mapped = mapper.Map<List<BookModel>>(records);
                await bookRepository.AddBooksAsync(mapped);

                await hubContext.Clients.All.SendAsync("ReceiveTableUpdate");

                return (true, $"Successfully uploaded {records.Count} books.");
            }
            else
                return (false, "No valid book data found in the CSV.");
            
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred while processing the file: {ex.Message}");
        }
    }
}

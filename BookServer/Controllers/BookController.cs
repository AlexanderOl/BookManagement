using BookServer.Interfaces;
using BookServer.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models;
using System.Formats.Asn1;
using System.Globalization;

namespace BookServer.Controllers;

public static class BookController
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        app.MapGet("/get-book/{id:guid}", GetBook);
        app.MapDelete("/del-book/{id:guid}", RemoveBook);
        app.MapPost("/add-book", UpsertBook);
        app.MapPut("/edit-book", UpsertBook);

        app.MapPost("/filter-books", FilterBooks);
        app.MapPost("/upload-books", UploadBooks).AllowAnonymous();

        app.MapGet("/antiforgery/token", AddAntiforgeryToken).ExcludeFromDescription();
    }

    private static async Task AddAntiforgeryToken(HttpContext context)
    {
        var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
        var tokens = antiforgery.GetAndStoreTokens(context);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync($"{{\"requestToken\":\"{tokens.RequestToken}\"}}");
    }
    private static async Task<IResult> UploadBooks(IFormFile file, IBookService bookService)
    {
        var (success, errorMsg) = await bookService.UploadCsvAsync(file);
        return success ? Results.Ok("Ok") : Results.Ok(errorMsg);
    }

    private static async Task<IResult> GetBook(IBookService bookService, Guid id)
    {
        BookView? book = await bookService.GetBookByIdAsync(id);
        return book == null ? Results.NotFound() : Results.Ok(book);
    }

    private static async Task<IResult> RemoveBook(IBookService bookService, Guid id)
    {
        bool success = await bookService.RemoveBookAsync(id);
        return success ? Results.BadRequest() : Results.Ok();
    }

    private static async Task<IResult> FilterBooks(IBookService bookService,
        BookFilter filter)
    {
        IEnumerable<BookView> tasks = await bookService.GetBooksAsync(filter);
        return Results.Ok(tasks);
    }

    private static async Task<IResult> UpsertBook(IBookService bookService,
        [FromBody] UpsertBookArg addArgs)
    {
        Guid newGuid = await bookService.UpsertBookAsync(addArgs);
        return Results.Ok(newGuid);
    }
}

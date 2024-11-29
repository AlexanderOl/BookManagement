using BookServer.Interfaces;
using BookServer.Repositories;
using BookServer.Services;
using Microsoft.EntityFrameworkCore;

namespace BookServer.Extensions;

public static class DiContainer
{
    public static void Register(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TaskDb"));
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IBookRepository, BookRepository>();
    }
}

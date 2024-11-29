using BookServer.Models;
using Microsoft.EntityFrameworkCore;

namespace BookServer.Services;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BookModel> Books { get; set; }
}

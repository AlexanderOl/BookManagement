using Shared.Enums;

namespace Shared.Models;

public class BookFilter
{
    public Genre? Genre { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
}

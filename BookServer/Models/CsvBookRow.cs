using Shared.Enums;

namespace BookServer.Models;

public class CsvBookRow
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required Genre Genre { get; set; }
}

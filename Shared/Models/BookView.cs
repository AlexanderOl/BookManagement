using Shared.Enums;

namespace Shared.Models;

public class BookView
{
    public Guid Id { get; set; }
    public required Genre Genre { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

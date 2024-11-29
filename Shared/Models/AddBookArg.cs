using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class UpsertBookArg
{
    public Guid? Id { get; set; }
    [Required(ErrorMessage = "Genre is required.")]
    public Genre? Genre { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(500, ErrorMessage = "Task title must be less than 500 characters.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Author is required.")]
    [StringLength(100, ErrorMessage = "Author must be less than 100 characters.")]
    public string? Author { get; set; }
}

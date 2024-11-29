using System.ComponentModel.DataAnnotations;

namespace Shared.Enums;

public enum Genre
{
    [Display(Name = "Action")]
    Action,
    [Display(Name = "Adventure")]
    Adventure,
    [Display(Name = "Dystopian")]
    Dystopian,
    [Display(Name = "Fantasy")]
    Fantasy,
    [Display(Name = "Horror")]
    Horror,
    [Display(Name = "Mystery")]
    Mystery,
    [Display(Name = "Thriller")]
    Thriller,
    [Display(Name = "Science Fiction")]
    ScienceFiction
}

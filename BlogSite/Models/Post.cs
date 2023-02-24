using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int Author { get; set; }

    [Required(ErrorMessage = "The title field is required!")]
    [StringLength(100,MinimumLength = 3, ErrorMessage = "The title must be between 3 and 100 characters in length.")]
    public string Title { get; set; } = null!;

    public string PathToContent { get; set; } = null!;

    [Required(ErrorMessage = "The theme field is required!")]
    [Display(Name = "Post's theme")]
    public int? Theme { get; set; }

    [Display(Name = "Created: ")]
    public DateTime CreationDate { get; set; }

    [Display(Name = "Updated: ")]
    public DateTime LastUpdateDate { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;

    public virtual Theme? ThemeNavigation { get; set; }

    public virtual ICollection<User> Likers { get; } = new List<User>();
}

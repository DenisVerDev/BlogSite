using System;
using System.Collections.Generic;

namespace BlogSite.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int Author { get; set; }

    public string Title { get; set; } = null!;

    public string PathToContent { get; set; } = null!;

    public string? Theme { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;

    public virtual Theme? ThemeNavigation { get; set; }

    public virtual ICollection<User> Likers { get; } = new List<User>();
}

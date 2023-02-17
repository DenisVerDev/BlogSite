using System;
using System.Collections.Generic;

namespace BlogSite.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Content { get; set; } = null!;

    public byte[]? FrontImage { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;

    public virtual ICollection<User> Likers { get; } = new List<User>();
}

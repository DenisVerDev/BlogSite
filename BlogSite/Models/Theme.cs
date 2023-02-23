using System;
using System.Collections.Generic;

namespace BlogSite.Models;

public partial class Theme
{
    public int ThemeId { get; set; }

    public string Name { get; set; } = null!;

    public byte[] ThemeImage { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}

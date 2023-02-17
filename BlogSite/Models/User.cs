using System;
using System.Collections.Generic;

namespace BlogSite.Models;

public partial class User
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Post> PostsNavigation { get; } = new List<Post>();

    public virtual ICollection<User> Authors { get; } = new List<User>();

    public virtual ICollection<User> Followers { get; } = new List<User>();

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}

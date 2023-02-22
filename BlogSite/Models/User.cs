using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models;

public partial class User
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "The username field is required!")]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "The username must be between 4 and 20 characters in length.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "The email address field is required!")]
    [MinLength(6, ErrorMessage = "The minimum length for an email address is 6 characters.")]
    [MaxLength(100, ErrorMessage = "The maximum length for an email address is 100 characters.")]
    [RegularExpression(@"\w+@\w+\.\w{2,}", ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "The password field is required!")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "The password must be between 6 and 20 characters in length.")]
    [RegularExpression("^[A-Za-z]*[0-9]+[A-Za-z0-9]*$", ErrorMessage = "The password must contain only English letters and at least one digit.")]
    public string Password { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; } = new List<Post>();

    public virtual ICollection<User> Authors { get; } = new List<User>();

    public virtual ICollection<User> Followers { get; } = new List<User>();

    public virtual ICollection<Post> LikedPosts { get; } = new List<Post>();
}

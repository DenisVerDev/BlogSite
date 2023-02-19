using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Models.ServerValidations
{
    public static class UserServerValidation
    {
        public static async Task<bool> SignInAsync(ModelStateDictionary ModelState, BlogSiteContext db, User user)
        {
            // 1st step: check if entered username is already taken
            if (db.Users.Find(user.Username) != null) ModelState.AddModelError("User.Username", "This username is already taken!");

            // 2nd step: check if entered email address is already taken
            if (db.Users.Where(x=>x.Email == user.Email).Any()) ModelState.AddModelError("User.Email", "This email address is already taken!");

            return ModelState.IsValid;
        }
    }
}

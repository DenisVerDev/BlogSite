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

        public static async Task<bool> LogInAsync(ModelStateDictionary ModelState, BlogSiteContext db, User user)
        {
            // 1st step: check if user with entered username exists
            // 2nd step: check if password is correct

            var db_user = db.Users.Find(user.Username);

            if (db_user == null) ModelState.AddModelError("User.Username", "There is no account with that username!");
            else  if (!db_user.Password.Equals(user.Password)) ModelState.AddModelError("User.Password", "Wrong password!");

            ModelState.Remove("User.Email"); // email is not neccesary in this validation

            return ModelState.IsValid;
        }
    }
}

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using BlogSite.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services
{
    public class ClientService
    {
        private const string TempKey = "Client";

        private readonly BlogSiteContext? db;

        private ITempDataDictionary TempData;

        public bool IsAuthenticated
        {
            get { return TempData.Peek(TempKey) != null; }
        }

        public ClientService(ITempDataDictionary TempData)
        {
            this.TempData = TempData;
            this.db = null;
        }

        public ClientService(ITempDataDictionary TempData, BlogSiteContext db)
        {
            this.TempData = TempData;
            this.db = db;
        }

        public User? GetDeserializedClient()
        {
            try
            {
                var userjson = TempData.Peek(TempKey);

                if (userjson != null) 
                    return JsonConvert.DeserializeObject<User>(userjson.ToString()!);
            }
            catch(Exception ex)
            {
                
            }

            return null;
        }

        public void SaveClient(User? client)
        {
            TempData[TempKey] = client != null ? JsonConvert.SerializeObject(client) : null;
        }

        public async Task<bool> LogInAsync(User LoginUser, ModelStateDictionary ModelState)
        {
            var db_user = await db.Users.Where(x => x.Email == LoginUser.Email).FirstOrDefaultAsync();

            if(db_user != null)
            {
                if (db_user.Password.Equals(LoginUser.Password)) this.SaveClient(db_user); // save in TempData to remember the authentication
                else ModelState.AddModelError($"{TempKey}.Password", "Wrong password!");
            }
            else ModelState.AddModelError($"{TempKey}.Email", "There is no account with that email!");

            ModelState.Remove($"{TempKey}.Username"); // username check is not necessary

            return ModelState.IsValid;
        }

        public async Task<bool> SignInAsync(User SigninUser, ModelStateDictionary ModelState)
        {
            bool userexisted = await db.Users.AnyAsync(x => x.Email == SigninUser.Email);
            
            if (!userexisted)
            {
                db.Users.Add(SigninUser);
                await db.SaveChangesAsync();

                this.SaveClient(SigninUser);
            }
            else ModelState.AddModelError($"{TempKey}.Email", "This email address is already taken!");

            return ModelState.IsValid;
        }

        public async Task UpdateAsync(User UpdateUser)
        {
            var entity = db.Users.Attach(UpdateUser);
            entity.State = EntityState.Modified;

            await db.SaveChangesAsync();

            this.SaveClient(UpdateUser);
        }

        // test version
        public async Task DeleteAsync(User DeleteUser)
        {
            this.SaveClient(null); // logout before delete operation

            await db.Users.Where(x => x.UserId == DeleteUser.UserId).SelectMany(x => x.Followers).ExecuteDeleteAsync();
            await db.Users.Where(x => x.UserId == DeleteUser.UserId).SelectMany(x => x.Authors).ExecuteDeleteAsync();
            await db.Users.Where(x => x.UserId == DeleteUser.UserId).SelectMany(x => x.LikedPosts).ExecuteDeleteAsync();
            await db.Posts.Where(x => x.Author == DeleteUser.UserId).ExecuteDeleteAsync();

            db.Users.Remove(DeleteUser);
            await db.SaveChangesAsync();
        }
    }
}

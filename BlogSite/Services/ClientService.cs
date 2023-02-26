using Microsoft.AspNetCore.Mvc.ViewFeatures;
using BlogSite.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogSite.Services
{
    public class ClientService
    {
        private const string TempClientKey = "Client";

        private ITempDataDictionary TempData;

        public bool IsAuthenticated
        {
            get { return TempData.Peek(TempClientKey) != null; }
        }

        public ClientService(ITempDataDictionary TempData)
        {
            this.TempData = TempData;
        }

        public User? GetDeserializedClient()
        {
            try
            {
                var userjson = TempData.Peek(TempClientKey);

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
            TempData[TempClientKey] = client != null ? JsonConvert.SerializeObject(client) : null;
        }

        public async Task<bool> LogInAsync(ModelStateDictionary ModelState, BlogSiteContext db, User LoginUser)
        {
            var db_user = db.Users.Where(x => x.Email == LoginUser.Email).FirstOrDefault();

            if(db_user != null)
            {
                if (db_user.Password.Equals(LoginUser.Password)) this.SaveClient(db_user);
                else ModelState.AddModelError($"{TempClientKey}.Password", "Wrong password!");
            }
            else ModelState.AddModelError($"{TempClientKey}.Email", "There is no account with that email!");

            ModelState.Remove($"{TempClientKey}.Username"); // username check is not necessary

            return ModelState.IsValid;
        }

        public async Task<bool> SignInAsync(ModelStateDictionary ModelState, BlogSiteContext db, User SigninUser)
        {
            if (!db.Users.Where(x => x.Email == SigninUser.Email).Any())
            {
                db.Users.Add(SigninUser);
                db.SaveChanges();

                this.SaveClient(SigninUser);
            }
            else ModelState.AddModelError($"{TempClientKey}.Email", "This email address is already taken!");

            return ModelState.IsValid;
        }
    }
}

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using BlogSite.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogSite.Services
{
    public class ClientService
    {
        private static ClientService service;
        private static object locker = new object();

        private const string TempClientKey = "Client";

        private ITempDataDictionary TempData;

        private User? client;

        public User? Client
        {
            get
            {
                if (client == null)
                    client = GetClient();

                return client;
            }

            private set
            {
                client = value;
                WriteClient();
            }
        }

        public bool IsAuthenticated
        {
            get { return client != null; }
        }

        private ClientService(ITempDataDictionary TempData)
        {
            this.TempData = TempData;
        }

        public static ClientService GetService(ITempDataDictionary TempData)
        {
            if (service == null)
            {
                lock (locker)
                {
                    if (service == null) service = new ClientService(TempData);
                }
            }
                
            return service;
        }

        //--------- Instance's methods down here-----------

        private User? GetClient()
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

        private void WriteClient()
        {
            if (client != null) TempData[TempClientKey] = JsonConvert.SerializeObject(client);
            else TempData.Remove(TempClientKey);
        }

        public void Logout()
        {
            Client = null;
        }

        public async Task<bool> LogInAsync(ModelStateDictionary ModelState, BlogSiteContext db, User LoginUser)
        {
            // 1st step: check if user with entered email address exists
            // 2nd step: check if password is correct
            // 3rd step: remove unnecessary check

            var db_user = db.Users.Where(x => x.Email == LoginUser.Email).FirstOrDefault();

            if(db_user == null) ModelState.AddModelError($"{TempClientKey}.Email", "There is no account with that email!");
            else if(!db_user.Password.Equals(LoginUser.Password))
            {
                db_user = null;
                ModelState.AddModelError($"{TempClientKey}.Password", "Wrong password!");
            }

            Client = db_user;

            ModelState.Remove($"{TempClientKey}.Username"); // username check is not necessary

            return ModelState.IsValid;
        }

        public async Task<bool> SignInAsync(ModelStateDictionary ModelState, BlogSiteContext db, User SigninUser)
        {
            if (!db.Users.Where(x => x.Email == SigninUser.Email).Any())
            {
                db.Users.Add(SigninUser);
                db.SaveChanges();

                Client = SigninUser;
            }
            else
            {
                Client = null;
                ModelState.AddModelError($"{TempClientKey}.Email", "This email address is already taken!");
            }

            return ModelState.IsValid;
        }
    }
}

using BlogSite.Models;
using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Author
{
    public class SettingsModel : PageModel
    {
        private readonly BlogSiteContext db;

        [BindProperty]
        public User Client { get; set; }

        public SettingsModel(BlogSiteContext db)
        {
            this.db = db;
        }

        public IActionResult OnGet()
        {
            ClientService clientService = new ClientService(TempData);
            this.Client = clientService.GetDeserializedClient();

            if (this.Client is null) 
                return RedirectToPage("/Authentication/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            try
            {
                ClientService clientService = new ClientService(TempData, this.db);
                var client = clientService.GetDeserializedClient();

                if (this.AnyChanges(client)) await clientService.UpdateAsync(this.Client!);

                return RedirectToPage();
            }
            catch(Exception ex)
            {
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                ClientService clientService = new ClientService(TempData, this.db);
                var client = clientService.GetDeserializedClient();

                if (client is null)
                    throw new Exception();

                await clientService.DeleteAsync(client);
            }
            catch(Exception ex)
            {
                return new JsonResult(false);
            }

            return new JsonResult(true);
        }

        private bool AnyChanges(User? client)
        {
            if(client != null)
            {
                // these properties are not for change, so they must stay the same
                this.Client.UserId = client.UserId;
                this.Client.Email = client.Email;

                return !this.Client.Equals(client); // if there is no changes => no update request to database
            }

            return false;
        }
    }
}
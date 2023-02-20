using BlogSite.Models;
using BlogSite.Models.ServerValidations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlogSite.Pages.Authentication
{
    public class IndexModel : PageModel
    {
        private BlogSiteContext db;

        [BindProperty]
        public User User { get; set; }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;
        }

        public IActionResult OnGet()
        {
            if (TempData.Peek("User") != null)
                return RedirectToPage("../Index");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if(await UserServerValidation.LogInAsync(ModelState,db,User))
                {
                    User = await db.Users.Where(x => x.Username == User.Username).SingleAsync();

                    TempData["User"] = JsonConvert.SerializeObject(User);

                    return RedirectToPage("../Index");
                }
            }
            catch(Exception ex)
            {
                ViewData["ServerExceptionData"] = new ServerExceptionData(ex);
            }

            return Page();
        }
    }
}

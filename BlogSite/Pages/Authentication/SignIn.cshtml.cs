using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogSite.Models;
using BlogSite.Models.ServerValidations;
using Newtonsoft.Json;

namespace BlogSite.Pages.Authentication
{
    public class SignInModel : PageModel
    {
        private readonly BlogSiteContext db;

        [BindProperty]
        public User User { get; set; }

        public SignInModel(BlogSiteContext db)
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
                if(await UserServerValidation.SignInAsync(ModelState,db, User))
                {
                    await db.Users.AddAsync(this.User);
                    await db.SaveChangesAsync();

                    TempData["User"] = JsonConvert.SerializeObject(this.User);

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

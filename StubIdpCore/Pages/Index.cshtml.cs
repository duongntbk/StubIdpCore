using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StubIdpCore.Pages
{
    public class IndexModel : PageModel
    {
        public ActionResult OnGet()
        {
            return LocalRedirect("/interactive/login");
        }
    }
}

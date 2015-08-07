using System.Web.Mvc;

namespace TotpAuthenticationExample.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(Session);
        }
    }
}
using TotpAuthenticationExample.Data;
using System.Web.Mvc;

namespace TotpAuthenticationExample.Controllers
{
    public class BaseController : Controller
    {
        protected static UserRepository Users
        {
            get
            {
                return RepositorySingletons.Users;
            }
        }

        protected static SessionRepository Sessions
        {
            get
            {
                return RepositorySingletons.Sessions;
            }
        }

        protected new Session Session { get; set; }

        protected new User User
        {
            get
            {
                if (Session == null)
                {
                    return null;
                }

                return Session.User;
            }
        }

        protected const string SessionIdCookieName = "session-id";

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sessionIdCookie = filterContext.HttpContext.Request.Cookies[SessionIdCookieName];
            if (sessionIdCookie != null)
            {
                Session = Sessions.Get(sessionIdCookie.Value);
            }

            base.OnActionExecuting(filterContext);
        }

        protected ActionResult RedirectHome()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
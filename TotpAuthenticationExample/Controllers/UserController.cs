using TotpAuthenticationExample.Data;
using System.Web;
using System.Web.Mvc;

namespace TotpAuthenticationExample.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Title = "Create Account";
            return View("AddOrAuthenticateForm");
        }

        [HttpPost]
        public ActionResult Add(string username, string password)
        {
            var user = Users.Get(username);

            if (user != null)
            {
                ModelState.AddModelError("user-exists", "user already exists");

                ViewBag.Title = "Create Account";
                return View("AddOrAuthenticateForm");
            }

            user = new User(username, password);

            Users.Add(user);
            CreateSession(user);

            return RedirectHome();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            ViewBag.Title = "Log In";
            return View("AddOrAuthenticateForm");
        }

        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            var user = Users.Get(username);

            if (user == null)
            {
                ModelState.AddModelError("no-user", "user does not exist");

                ViewBag.Title = "Log In";
                return View("AddOrAuthenticateForm");
            }

            if (user.Password != password)
            {
                ModelState.AddModelError("bad-password", "wrong password");

                ViewBag.Title = "Log In";
                return View("AddOrAuthenticateForm");
            }

            CreateSession(user);

            if (Session.NeedsToBeTotpAuthenticated)
            {
                return RedirectToAction("Verify", "TotpAuthentication");
            }

            return RedirectHome();
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            if (Session != null)
            {
                Session.Expire();
            }

            return RedirectHome();
        }

        private void CreateSession(User user)
        {
            Session = new Session(user);
            Sessions.Add(Session);

            Response.Cookies.Set(new HttpCookie(SessionIdCookieName)
            {
                Value = Session.Id
            });
        }
    }
}
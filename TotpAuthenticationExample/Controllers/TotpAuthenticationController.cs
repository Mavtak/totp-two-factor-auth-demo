using System.Web;
using System.Web.Mvc;
using Base32;
using OtpSharp;

namespace TotpAuthenticationExample.Controllers
{
    public class TotpAuthenticationController : BaseController
    {
        private const string AppName = "Mavtak's Proof of Concept App";

        [HttpGet]
        public ActionResult Enable()
        {
            if (User == null)
            {
                ModelState.AddModelError("not-authenticated", "you are not logged in");
                return View();
            }

            if (User.TotpSecret != null)
            {
                ModelState.AddModelError("already-enabled", "Google Authenticator is already enabled");
                return View();
            }

            var secretKey = KeyGeneration.GenerateRandomKey();

            ViewBag.SecretKey = Base32Encoder.Encode(secretKey);
            ViewBag.BarcodeUrl = GenerateBarcodeUrl(secretKey); ;

            return View();
        }

        [HttpPost]
        public ActionResult Enable(string secretKey, string code)
        {
            if (User == null)
            {
                ModelState.AddModelError("not-authenticated", "you are not logged in");
                return View();
            }

            if (User.TotpSecret != null)
            {
                ModelState.AddModelError("already-enabled", "Google Authenticator is already enabled");
                return View();
            }

            var secretKeyBytes = Base32Encoder.Decode(secretKey);
            if (!VerifyCode(secretKeyBytes, code))
            {
                ModelState.AddModelError("wrong-code", "The code is not valid");

                ViewBag.SecretKey = secretKey;
                ViewBag.BarcodeUrl = GenerateBarcodeUrl(secretKeyBytes);
                return View();
            }

            User.TotpSecret = secretKey;
            Session.TotpAuthenticated = true;

            return RedirectHome();
        }

        [HttpGet]
        public ActionResult Disable()
        {
            if (User == null)
            {
                ModelState.AddModelError("not-authenticated", "you are not logged in");
                return View("Verify");
            }

            if (Session.NeedsToBeTotpAuthenticated)
            {
                ModelState.AddModelError("not-verified", "you have not verified your Google Authenticator token");
                return View("Verify");
            }

            User.TotpSecret = null;

            return RedirectHome();
        }

        [HttpGet]
        public ActionResult Verify()
        {
            if (User == null)
            {
                ModelState.AddModelError("not-authenticated", "you are not logged in");
                return View();
            }

            if (User.TotpSecret == null)
            {
                ModelState.AddModelError("not-enabled", "Google Authenticator is not enabled");
                return View();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Verify(string code)
        {
            if (User == null)
            {
                ModelState.AddModelError("not-authenticated", "you are not logged in");
                return View();
            }

            if (User.TotpSecret == null)
            {
                ModelState.AddModelError("not-enabled", "Google Authenticator is not enabled");
                return View();
            }

            var secretKeyBytes = Base32Encoder.Decode(User.TotpSecret);
            if (!VerifyCode(secretKeyBytes, code))
            {
                ModelState.AddModelError("wrong-code", "the code that you entered is not valid");
                return View();
            }

            Session.TotpAuthenticated = true;
            return RedirectHome();
        }

        private string GenerateBarcodeUrl(byte[] secretKey)
        {
            return string.Format("otpauth://totp/{0}?secret={1}&issuer={2}",
                HttpUtility.UrlEncode(User.Name),
                Base32Encoder.Encode(secretKey),
                HttpUtility.UrlEncode(AppName)
            );
        }

        private bool VerifyCode(byte[] secretKey, string code)
        {
            long timeStepMatched;
            var otp = new Totp(secretKey);
            return otp.VerifyTotp(code, out timeStepMatched, new VerificationWindow(2, 2));
        }
    }
}
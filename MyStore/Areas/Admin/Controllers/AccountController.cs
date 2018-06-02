using FX.Context.IdentityDomain;
using FX.Core.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MyStore.Areas.Admin.Models;
using MyStore.Context.IdentityConfiguration;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace MyStore.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private const int height = 30;
        private const int width = 80;
        private const int length = 4;

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            string randomText = string.Empty;
            var bmpBytes = GetCatcha(out randomText);
            var hash = Compute.ComputeMd5Hash(randomText + GetSalt());
            Session["CaptchaHash"] = hash;
            var model = new LoginViewModel(bmpBytes, string.Empty, string.Empty, string.Empty);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userName, string password, string captcha, string returnUrl)
        {
            var hash = Compute.ComputeMd5Hash(captcha + GetSalt());
            var captchaHash = Session["CaptchaHash"] as string;
            if (hash.Equals(captchaHash))
            {
                ApplicationUser user = UserManager.Instance.UserManagerment.Find(userName, password);
                if (user != null)
                {
                    if (!user.Status)
                    {
                        ViewData["ErrorLogin"] = "Tài khoản này đang bị khóa.";
                    }
                    else
                    {
                        IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                        authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                        ClaimsIdentity identity = UserManager.Instance.UserManagerment.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationProperties props = new AuthenticationProperties();
                        authenticationManager.SignIn(props, identity);
                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    ViewData["ErrorLogin"] = "Sai tên đăng nhập hoặc mật khẩu.";
                }
            }
            else
            {
                ViewData["ErrorLogin"] = "Nhập sai mã xác thực";
            }
            string randomText = string.Empty;
            var bmpBytes = GetCatcha(out randomText);
            Session["CaptchaHash"] = Compute.ComputeMd5Hash(randomText + GetSalt());
            var model = new LoginViewModel(bmpBytes, userName, password, captcha);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private static void DrawRandomLines(ref Graphics g, int width, int height)
        {
            var rnd = new Random();
            var pen = new Pen(Color.Gray);
            for (var i = 0; i < 10; i++)
            {
                g.DrawLine(pen, rnd.Next(0, width), rnd.Next(0, height),
                                rnd.Next(0, width), rnd.Next(0, height));
            }
        }

        private static string GetSalt()
        {
            return typeof(AccountController).Assembly.FullName;
        }

        private static byte[] GetCatcha(out string captchaText)
        {
            var randomText = TextHelper.GenerateRandomText(4);
            var rnd = new Random();
            var fonts = new[] { "Verdana", "Times New Roman" };
            float orientationAngle = rnd.Next(0, 359);

            var index0 = rnd.Next(0, fonts.Length);
            var familyName = fonts[index0];

            using (var bmpOut = new Bitmap(width, height))
            {
                var g = Graphics.FromImage(bmpOut);
                var gradientBrush = new LinearGradientBrush(new Rectangle(0, 0, width, height),
                                                            Color.White, Color.DarkGray,
                                                            orientationAngle);
                g.FillRectangle(gradientBrush, 0, 0, width, height);
                DrawRandomLines(ref g, width, height);
                g.DrawString(randomText, new Font(familyName, 18), new SolidBrush(Color.LightSlateGray), 0, 2);
                var ms = new MemoryStream();
                bmpOut.Save(ms, ImageFormat.Png);
                var bmpBytes = ms.GetBuffer();
                bmpOut.Dispose();
                ms.Close();
                captchaText = randomText;
                return bmpBytes;
            }
        }
    }
}
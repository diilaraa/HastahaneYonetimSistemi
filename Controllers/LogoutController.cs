using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjeYonetim.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Tüm oturum verilerini temizle
            return RedirectToAction("Login", "Account"); // Giriş sayfasına yönlendir
        }
    }
}
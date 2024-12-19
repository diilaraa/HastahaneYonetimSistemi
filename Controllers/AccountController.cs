using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjeYonetim.Models;
using ProjeYonetim.Models.Data;

namespace ProjeYonetim.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        DataBaseContext db = new DataBaseContext();
        // Admin kullanıcısını veritabanına eklemek için
        public ActionResult CreateAdmin()
        {
            // Veritabanında bir Admin var mı kontrol edelim
            var adminExists = db.Adminler.Any();

            // Eğer Admin yoksa yeni bir Admin ekleyelim
            if (!adminExists)
            {
                var admin = new Admin
                {
                    Ad = "Esin",
                    Soyad = "Ay",
                    Mail = "esinay@example.com",
                    Sifre = "123456"
                };

                // Admin'i veritabanına ekliyoruz
                db.Adminler.Add(admin);
                db.SaveChanges();

                ViewBag.Message = "Admin kullanıcısı başarıyla oluşturuldu.";
            }
            else
            {
                ViewBag.Message = "Zaten bir admin kullanıcısı mevcut.";
            }

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(string mail, string sifre)
        {
            // Admin tablosunda kullanıcıyı doğrula
            var admin = db.Adminler.FirstOrDefault(a => a.Mail == mail && a.Sifre == sifre);

            if (admin != null)
            {
                // Kullanıcı doğrulandı, session ile oturum başlat
                Session["AdminId"] = admin.AdminId;
                Session["AdminAdSoyad"] = $"{admin.Ad} {admin.Soyad}";
                Session["IsAdmin"] = true; // Admin olduğunu işaretle

                // Admin paneline yönlendir
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["IsAdmin"] = null; // Varsayılan durumu koru
                ViewBag.ErrorMessage = "E-posta veya şifre hatalı.";
            }
            return View();
            // Hatalı giriş, hata mesajı gönder

        }
    }
}
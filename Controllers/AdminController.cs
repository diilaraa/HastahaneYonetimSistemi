using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjeYonetim.Filters;
using ProjeYonetim.Models.Data;

namespace ProjeYonetim.Controllers
{
    [AdminAuthorize]
    public class AdminController : BaseController
    {
        DataBaseContext db = new DataBaseContext();

        // Admin paneline giriş yapmadan erişimi engellemek için bir kontrol ekleyin
        protected bool IsAdmin
        {
            get { return (bool)(Session["IsAdmin"] ?? false); }
            set { Session["IsAdmin"] = value; }
        }

        public ActionResult Index()
        {
            if (IsAdmin)
            {
                // Sadece admin için işlem yap
            }
            else
            {
                return RedirectToAction("Index", "Home"); // Kullanıcıları ana sayfaya yönlendir
            }
            return View();
        }


    }
}
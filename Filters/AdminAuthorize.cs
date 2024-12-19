using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjeYonetim.Filters
{
    public class AdminAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Session'dan IsAdmin durumunu kontrol et
            var session = filterContext.HttpContext.Session;

            bool isAdmin = session["IsAdmin"] != null && (bool)session["IsAdmin"];

            if (isAdmin)
            {
                // Eğer admin ise, adminler için ekstra bir şey yapılmak istenirse buraya eklenebilir.
                base.OnActionExecuting(filterContext);
                return;
            }

            // Eğer admin değilse sayfanın kullanıcılar için görünür olmasına izin ver
            // Ancak ek olarak farklı kontrol uygulanabilir (isteğe bağlı)
            base.OnActionExecuting(filterContext);
        }
    }
}
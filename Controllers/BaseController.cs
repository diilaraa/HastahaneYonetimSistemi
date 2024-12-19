using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjeYonetim.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public bool IsAdmin
        {
            get { return (bool)(Session["IsAdmin"] ?? false); }
        }
    }
}
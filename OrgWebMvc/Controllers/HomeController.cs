using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgWebMvc.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            UserValid();
            return View();
        }

        public ActionResult About()
        {
            UserValid();
            ViewBag.Message = "This is the organization web application prototype";

            return View();
        }

        public ActionResult Contact()
        {
            UserValid();
            ViewBag.Message = "Our website: http://www.thelibraryapp.gearhostpreview.com";

            return View();
        }
    }
}
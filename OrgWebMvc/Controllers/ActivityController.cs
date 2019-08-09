using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgWebMvc.Controllers
{
    public class ActivityController : BaseController
    {
        // GET: Activity
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TimeLine()
        {
            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
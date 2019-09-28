using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgWebMvc.Controllers
{
    public class PublicController : BaseController
    {
        // GET: Public
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Article()
        {
            return View();
        }
    }
}
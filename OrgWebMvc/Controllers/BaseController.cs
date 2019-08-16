using OrgWebMvc.Main.Service;
using OrgWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgWebMvc.Controllers
{
    public class BaseController : Controller
    {
        protected UserService UserService;
        protected user LoggedUser;

        public BaseController()
        {
            UserService = new UserService();

        }

        public bool ClearUserSession()
        {
            if (HttpContext.Session != null && HttpContext.Session["loggedUser"] != null)
            {
                HttpContext.Session.Clear();
            }
            return !UserValid();
        }

        public bool UserValid()
        {

            if (HttpContext.Session != null && HttpContext.Session["loggedUser"] != null)
            {
                object ObjSession = Session["loggedUser"];

                user UserCheck = (user)ObjSession;
                object UserDB = UserService.GetUser(UserCheck.username, UserCheck.password);
                if (UserDB != null)
                {
                    LoggedUser = (user)UserDB;
                    ViewData["Name"] = LoggedUser.name;
                    ViewData["IsAdmin"] = LoggedUser.admin == 1;
                    ViewData["InstitutionName"] = LoggedUser.institution.name;
                    return true;
                }
                else
                {
                    ViewData.Clear();
                    LoggedUser = null;
                    return false;
                }

            }
            else
            {
                LoggedUser = null;
                return false;
            }
        }
    }
}
using InstApp.Util.Common;
using OrgWebMvc.Main.API;
using OrgWebMvc.Main.Service;
using OrgWebMvc.Main.Util;
using OrgWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace OrgWebMvc.Controllers
{
    public class EntityController : BaseController
    {
        // GET: Entity
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Division()
        {
            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            DivisionService DivisionSvc = new DivisionService();

            ViewBag.Title = "Division";
            ViewData = MVCUtil.PopulateCRUDViewData(typeof(division), "Division", DivisionSvc, Request, ViewData);
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult Program()
        {
            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            ProgramService ProgramSvc = new ProgramService();

            ViewBag.Title = "Program";
            ViewData = MVCUtil.PopulateCRUDViewData(typeof(program), "Program", ProgramSvc, Request, ViewData);
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult Event()
        {
            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            EventService EventSvc = new EventService();

            //enable timeline
            ViewData = MVCUtil.EnableTimeLine("Event", "name", "date", ViewData);
            //ViewData["EnableTimeLine"] = true;
            //ViewData["Entity"] = "Event";
            //ViewData["DateId"] = "date";
            //

            ViewBag.Title = "Event";
            ViewData = MVCUtil.PopulateCRUDViewData(typeof(@event), "Event", EventSvc, Request, ViewData);
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult Member()
        {
            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            MemberService MemberSvc = new MemberService();

            ViewBag.Title = "Member";
            ViewData = MVCUtil.PopulateCRUDViewData(typeof(member), "Member", MemberSvc, Request, ViewData);
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult Position()
        {
            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            PositionService PositionSvc = new PositionService();
            ViewBag.Title = "Position";
            ViewData = MVCUtil.PopulateCRUDViewData(typeof(position), "Position", PositionSvc, Request, ViewData);
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult Section()
        {
            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            SectionService SectionSvc = new SectionService();

            ViewBag.Title = "Section";
            ViewData = MVCUtil.PopulateCRUDViewData(typeof(section), "Section", SectionSvc, Request, ViewData);
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult Post()
        {
            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            PostService PostSvc = new PostService();

            ViewData = MVCUtil.EnableTimeLine("Post", "title", "date", ViewData);
            ViewBag.Title = "Post";
            ViewData = MVCUtil.PopulateCRUDViewData(typeof(post), "Post", PostSvc, Request, ViewData);
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult UserMng()
        {
            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            UserService UserSvc = new UserService();

            ViewBag.Title = "User";
            ViewData = MVCUtil.PopulateCRUDViewData(typeof(user), "User", UserSvc, Request, ViewData);
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        /**API**/

        [HttpPost]
        public ActionResult DivisionSvc()
        {
            bool Access = UserValid() && IsAdmin();
            WebResponse Response = new WebResponse();
            if (!Access || !StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            DivisionService EntitySvc = new DivisionService();
            switch (Action)
            {
                case "List":
                    Response = MVCUtil.generateResponseList(EntitySvc, Request, LoggedUser, new string[]
                            {
                            "id","name","description","institution_id"
                            }, typeof(division));

                    break;
                case "Form":
                    Response = MVCUtil.generateResponseWithForm(typeof(division), EntitySvc, Request);
                    break;
                case "Post":
                    division Division = (division)ObjectUtil.FillObjectWithMap(new division(), BaseService.ReqToDict(Request));
                    if (Division != null)
                    {
                        Division.institution_id = LoggedUser.institution_id;

                        string[] ObjParamToSend = new string[]{
                            "id","name","description","institution_id"
                        };
                        Response = MVCUtil.UpdateEntity(EntitySvc, Division, ObjParamToSend, Response);

                    }
                    break;
                case "Delete":
                    Response = MVCUtil.DeleteEntity(EntitySvc, Request, Response);
                    break;
                default:
                    break;
            }
            return Json(Response);
        }

        [HttpPost]
        public ActionResult ProgramSvc()
        {
            bool Access = UserValid();
            WebResponse Response = new WebResponse();
            if (!Access || !StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            ProgramService EntitySvc = new ProgramService();
            switch (Action)
            {
                case "List":
                    Response = MVCUtil.generateResponseList(EntitySvc, Request, LoggedUser, new string[]
                            {
                            "id","name","description","sect_id"
                         }, typeof(program));
                    break;
                case "Form":
                    Response = MVCUtil.generateResponseWithForm(typeof(program), EntitySvc, Request);
                    break;
                case "Post":
                    program Program = (program)ObjectUtil.FillObjectWithMap(new program(), BaseService.ReqToDict(Request));
                    if (Program != null)
                    {
                        string[] ObjParamToSend = new string[]{
                            "id","name","description","sect_id"
                        };
                        Response = MVCUtil.UpdateEntity(EntitySvc, Program, ObjParamToSend, Response);
                    }
                    break;
                case "Delete":
                    Response = MVCUtil.DeleteEntity(EntitySvc, Request, Response);
                    break;
                default:
                    break;
            }
            return Json(Response);
        }

        [HttpPost]
        public ActionResult EventSvc()
        {
            bool Access = UserValid();
            WebResponse Response = new WebResponse();
            if (!Access || !StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            EventService EntitySvc = new EventService();
            switch (Action)
            {
                case "List":
                    Response = MVCUtil.generateResponseList(EntitySvc, Request, LoggedUser, new string[]
                           {
                            "id","program_id","user_id","date","location","participant","info","done","name"
                        }, typeof(@event));
                    break;
                case "Form":
                    Response = MVCUtil.generateResponseWithForm(typeof(@event), EntitySvc, Request);
                    break;
                case "Post":
                    @event Event = (@event)ObjectUtil.FillObjectWithMap(new @event(), BaseService.ReqToDict(Request));
                    if (Event != null)
                    {
                        Event.user_id = LoggedUser.id;
                        string[] ObjParamToSend = new string[]{
                            "id","program_id","user_id","date","location","participant","info","done","name"
                        };
                        Response = MVCUtil.UpdateEntity(EntitySvc, Event, ObjParamToSend, Response);

                    }
                    break;
                case "Delete":
                    Response = MVCUtil.DeleteEntity(EntitySvc, Request, Response);
                    break;
                default:
                    break;
            }
            return Json(Response);
        }

        [HttpPost]
        public ActionResult MemberSvc()
        {
            bool Access = UserValid() && IsAdmin();
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            MemberService EntitySvc = new MemberService();
            switch (Action)
            {
                case "List":
                    Response = MVCUtil.generateResponseList(EntitySvc, Request, LoggedUser, new string[]
                           {
                            "id","name","description","position_id"
                        }, typeof(member));
                    break;
                case "Form":
                    Response = MVCUtil.generateResponseWithForm(typeof(member), EntitySvc, Request);
                    break;
                case "Post":
                    member Member = (member)ObjectUtil.FillObjectWithMap(new member(), BaseService.ReqToDict(Request));
                    if (Member != null)
                    {
                        Response = MVCUtil.UpdateEntity(EntitySvc, Member, new string[]{
                            "id","name","description","position_id"
                        }, Response);
                    }
                    break;
                case "Delete":
                    Response = MVCUtil.DeleteEntity(EntitySvc, Request, Response);
                    break;
                default:
                    break;
            }
            return Json(Response);
        }

        [HttpPost]
        public ActionResult SectionSvc()
        {
            bool Access = UserValid() && IsAdmin();
            WebResponse Response = new WebResponse();
            if (!Access || !StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            SectionService EntitySvc = new SectionService();
            switch (Action)
            {
                case "List":
                    Response = MVCUtil.generateResponseList(EntitySvc, Request, LoggedUser, new string[]
                    {
                            "id","name","description","division_id","parent_section_id"
                    }, typeof(section));
                    break;
                case "Form":
                    Response = MVCUtil.generateResponseWithForm(typeof(section), EntitySvc, Request);
                    break;
                case "Post":
                    section section = (section)ObjectUtil.FillObjectWithMap(new section(), BaseService.ReqToDict(Request));
                    if (section != null)
                    {
                        Response = MVCUtil.UpdateEntity(EntitySvc, section, new string[]{
                           "id","name","description","division_id","parent_section_id"
                        }, Response);
                    }
                    break;
                case "Delete":
                    Response = MVCUtil.DeleteEntity(EntitySvc, Request, Response);
                    break;
                default:
                    break;
            }
            return Json(Response);
        }

        [HttpPost]
        public ActionResult PositionSvc()
        {
            bool Access = UserValid() && IsAdmin();
            WebResponse Response = new WebResponse();
            if (!Access || !StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            PositionService EntitySvc = new PositionService();
            switch (Action)
            {
                case "List":
                    Response = MVCUtil.generateResponseList(EntitySvc, Request, LoggedUser, new string[]
                    {
                            "id","name","description","section_id","parent_position_id"
                    }, typeof(position));
                    break;
                case "Form":
                    Response = MVCUtil.generateResponseWithForm(typeof(position), EntitySvc, Request);
                    break;
                case "Post":
                    position position = (position)ObjectUtil.FillObjectWithMap(new position(), BaseService.ReqToDict(Request));
                    if (position != null)
                    {
                        Response = MVCUtil.UpdateEntity(EntitySvc, position, new string[]{
                           "id","name","description","section_id","parent_position_id"
                        }, Response);
                    }
                    break;
                case "Delete":
                    Response = MVCUtil.DeleteEntity(EntitySvc, Request, Response);
                    break;
                default:
                    break;
            }
            return Json(Response);
        }

        [HttpPost]
        public async Task<ActionResult> PostSvc()
        {
            bool Access = UserValid();
            WebResponse Response = new WebResponse();
            if (/*!Access || */!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            PostService EntitySvc = new PostService();

            Dictionary<string, object> PostProps = new Dictionary<string, object>()
            {
                { "id", null },
                { "user_id", null },
                { "title", null },
                { "body", null },
                { "date",null },
                 { "user",new Dictionary<string,object> { {"name", null } } },
                { "type",null },
                { "post_id",null }
            };

            switch (Action)
            {
                case "List":
                    string Scope = Request.Form["Scope"];
                    bool isPublic = (Scope != null && Scope.Equals("Public"))
                   ;
                    Response = MVCUtil.generateResponseList(EntitySvc, Request, LoggedUser, PostProps, typeof(post), isPublic);
                    break;
                case "Form":
                    if (!Access) return Json(Response);
                    Response = MVCUtil.generateResponseWithForm(typeof(post), EntitySvc, Request);
                    break;
                case "Post":
                    if (!Access) return Json(Response);
                    post post = (post)ObjectUtil.FillObjectWithMap(new post(), BaseService.ReqToDict(Request));
                    post.user_id = LoggedUser.id;
                    if (post != null)
                    {
                        Response = MVCUtil.UpdateEntity(EntitySvc, post, new string[]{
                           "id","user_id","title","body","date","type","post_id"
                        }, Response);
                    }
                    break;
                case "Delete":
                    if (!Access) return Json(Response);
                    Response = MVCUtil.DeleteEntity(EntitySvc, Request, Response);
                    break;
                case "Latest":
                    // if (!Access) return Json(Response);
                    string DateStr = Request.Form["timestamp"];
                    post CurrentPost = EntitySvc.findLatestPost();
                    if (CurrentPost != null)
                    {
                        DateTime Date = CurrentPost.created_date;
                        DateStr = DateStr.Replace("T", " ");
                        DateStr = DateStr.Replace("Z", "");
                        DateStr = DateStr.Replace("-", "");
                        try
                        {
                            Date = DateTime.ParseExact(DateStr, "yyyyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        }catch(Exception ex)
                       {
                            Date = CurrentPost.created_date;
                        }
                        post LatestPost = new post();
                        DateTime requestTime = DateTime.Now;
                        DateTime runningTime;

                        Boolean update = true;
                        bool hasUpdate = Date <= CurrentPost.created_date;
                        while (CurrentPost.created_date <= Date  )
                        {
                            runningTime = DateTime.Now;
                            LatestPost = EntitySvc.findLatestPost();
                            if (LatestPost != null)
                                Date = LatestPost.created_date;
                            TimeSpan deltaTime = runningTime - requestTime;
                            if (deltaTime.TotalMilliseconds >= 6000.0)
                            {
                                update = false;
                                break;
                            }
                        }
                        
                        Response = new WebResponse(update ? 0 : 1, StringUtil.DateTimeToString(DateTime.Now),
                            ObjectUtil.GetObjectValues(new string[] { "id", "title" }, LatestPost));
                    }
                    else
                    {
                        Response = new WebResponse(0, "NoUpdate");
                    }
                    break;
                default:
                    break;
            }
            return Json(Response);
        }

        [HttpPost]
        public ActionResult UserSvc()
        {
            bool Access = UserValid() && IsAdmin();
            WebResponse Response = new WebResponse();
            if (LoggedUser.admin != 1 || !Access || !StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            UserService EntitySvc = new UserService();
            switch (Action)
            {
                case "List":
                    Response = MVCUtil.generateResponseList(EntitySvc, Request, LoggedUser, new string[]
                    {
                        "id","name","username","password","email"
                    }, typeof(user));
                    break;
                case "Form":
                    Response = MVCUtil.generateResponseWithForm(typeof(user), EntitySvc, Request);
                    break;
                case "Post":
                    user User = (user)ObjectUtil.FillObjectWithMap(new user(), BaseService.ReqToDict(Request));
                    User.institution_id = LoggedUser.institution_id;
                    if (User != null)
                    {
                        Response = MVCUtil.UpdateEntity(EntitySvc, User, new string[]{
                          "id","name","username","password","email"
                        }, Response);
                    }
                    break;
                case "Delete":
                    Response = MVCUtil.DeleteEntity(EntitySvc, Request, Response);
                    break;
                default:
                    break;
            }
            return Json(Response);
        }


    }
}

using InstApp.Util.Common;
using OrgWebMvc.Main.API;
using OrgWebMvc.Main.Service;
using OrgWebMvc.Main.Util;
using OrgWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            ViewBag.Title = "Post";
            ViewData = MVCUtil.PopulateCRUDViewData(typeof(post), "Post", PostSvc, Request, ViewData);
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        /**API**/

        [HttpPost]
        public ActionResult DivisionSvc()
        {
            bool UserIsLoggedIn = UserValid();
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            DivisionService EntitySvc = new DivisionService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(EntitySvc, Request, LoggedUser);
                    List<division> Divisions = (List<division>)ObjectUtil.ConvertList(ObjList, typeof(List<division>));
                    List<division> ListToSend = new List<division>();
                    foreach (division D in Divisions)
                    {
                        division Div = (division)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","name","description","user_id"
                        }, D);
                        ListToSend.Add(Div);
                    }
                    object ResponseData = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Type"]) && Request.Form["Type"].ToString().Equals("JSONList"))
                    {
                        ResponseData = ListToSend;
                    }
                    else
                    {
                        ResponseData = CustomHelper.GenerateDataTableString(typeof(division), ObjList);
                    }
                    Response = new WebResponse(0, "Success", ResponseData, EntitySvc.count);
                    break;
                case "Form":
                    Response = MVCUtil.generateResponseWithForm(typeof(division), EntitySvc, Request);
                    break;
                case "Post":
                    division Division = (division)ObjectUtil.FillObjectWithMap(new division(), BaseService.ReqToDict(Request));
                    if (Division != null)
                    {
                        Division.user_id = LoggedUser.id;

                        string[] ObjParamToSend = new string[]{
                            "id","name","description","user_id"
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
            bool UserIsLoggedIn = UserValid();
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            ProgramService EntitySvc = new ProgramService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(EntitySvc, Request, LoggedUser);
                    List<program> programs = (List<program>)ObjectUtil.ConvertList(ObjList, typeof(List<program>));
                    List<program> ListToSend = new List<program>();
                    foreach (program P in programs)
                    {
                        program Div = (program)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","name","description","sect_id"
                        }, P);
                        ListToSend.Add(Div);
                    }
                    object ResponseData = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Type"]) && Request.Form["Type"].ToString().Equals("JSONList"))
                    {
                        ResponseData = ListToSend;
                    }
                    else
                    {
                        ResponseData = CustomHelper.GenerateDataTableString(typeof(program), ObjList);
                    }
                    Response = new WebResponse(0, "Success", ResponseData, EntitySvc.count); break;
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
            bool UserIsLoggedIn = UserValid();
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            EventService EntitySvc = new EventService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(EntitySvc, Request, LoggedUser);
                    List<@event> Events = (List<@event>)ObjectUtil.ConvertList(ObjList, typeof(List<@event>));
                    List<@event> ListToSend = new List<@event>();
                    foreach (@event E in Events)
                    {
                        @event Ev = (@event)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","program_id","user_id","date","location","participant","info","done","name"
                        }, E);
                        ListToSend.Add(Ev);
                    }
                    object ResponseData = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Type"]) && Request.Form["Type"].ToString().Equals("JSONList"))
                    {
                        ResponseData = ListToSend;
                    }
                    else
                    {
                        ResponseData = CustomHelper.GenerateDataTableString(typeof(@event), ObjList);
                    }
                    Response = new WebResponse(0, "Success", ResponseData, EntitySvc.count); break;
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
            bool UserIsLoggedIn = UserValid();
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
                    List<object> ObjList = BaseService.GetObjectList(EntitySvc, Request, LoggedUser);
                    List<member> Members = (List<member>)ObjectUtil.ConvertList(ObjList, typeof(List<member>));
                    List<member> ListToSend = new List<member>();
                    foreach (member Obj in Members)
                    {
                        member Mmb = (member)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","name","description","position_id"
                        }, Obj);
                        ListToSend.Add(Mmb);
                    }
                    object ResponseData = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Type"]) && Request.Form["Type"].ToString().Equals("JSONList"))
                    {
                        ResponseData = ListToSend;
                    }
                    else
                    {
                        ResponseData = CustomHelper.GenerateDataTableString(typeof(member), ObjList);
                    }
                    Response = new WebResponse(0, "Success", ResponseData, EntitySvc.count);
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
            bool UserIsLoggedIn = UserValid();
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            SectionService EntitySvc = new SectionService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(EntitySvc, Request, LoggedUser);
                    List<section> sections = (List<section>)ObjectUtil.ConvertList(ObjList, typeof(List<section>));
                    List<section> ListToSend = new List<section>();
                    foreach (section Obj in sections)
                    {
                        section Mmb = (section)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","name","description","division_id","parent_section_id"
                        }, Obj);
                        ListToSend.Add(Mmb);
                    }
                    object ResponseData = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Type"]) && Request.Form["Type"].ToString().Equals("JSONList"))
                    {
                        ResponseData = ListToSend;
                    }
                    else
                    {
                        ResponseData = CustomHelper.GenerateDataTableString(typeof(section), ObjList);
                    }
                    Response = new WebResponse(0, "Success", ResponseData, EntitySvc.count);
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
            bool UserIsLoggedIn = UserValid();
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            PositionService EntitySvc = new PositionService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(EntitySvc, Request, LoggedUser);
                    List<position> positions = (List<position>)ObjectUtil.ConvertList(ObjList, typeof(List<position>));
                    List<position> ListToSend = new List<position>();
                    foreach (position Obj in positions)
                    {
                        position Mmb = (position)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","name","description","section_id","parent_position_id"
                        }, Obj);
                        ListToSend.Add(Mmb);
                    }
                    object ResponseData = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Type"]) && Request.Form["Type"].ToString().Equals("JSONList"))
                    {
                        ResponseData = ListToSend;
                    }
                    else
                    {
                        ResponseData = CustomHelper.GenerateDataTableString(typeof(position), ObjList);
                    }
                    Response = new WebResponse(0, "Success", ResponseData, EntitySvc.count);
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
        public ActionResult PostSvc()
        {
            bool UserIsLoggedIn = UserValid();
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            PostService EntitySvc = new PostService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(EntitySvc, Request, LoggedUser);
                    List<post> posts = (List<post>)ObjectUtil.ConvertList(ObjList, typeof(List<post>));
                    List<post> ListToSend = new List<post>();
                    foreach (post Obj in posts)
                    {
                        post Post = (post)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","user_id","title","body","date","type","post_id"
                        }, Obj);
                        ListToSend.Add(Post);
                    }
                    object ResponseData = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Type"]) && Request.Form["Type"].ToString().Equals("JSONList"))
                    {
                        ResponseData = ListToSend;
                    }
                    else
                    {
                        ResponseData = CustomHelper.GenerateDataTableString(typeof(post), ObjList);
                    }
                    Response = new WebResponse(0, "Success", ResponseData, EntitySvc.count);
                    break;
                case "Form":
                    Response = MVCUtil.generateResponseWithForm(typeof(post), EntitySvc, Request);
                    break;
                case "Post":
                    post post = (post)ObjectUtil.FillObjectWithMap(new post(), BaseService.ReqToDict(Request));
                    if (post != null)
                    {
                        Response = MVCUtil.UpdateEntity(EntitySvc, post, new string[]{
                           "id","user_id","title","body","date","type","post_id"
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

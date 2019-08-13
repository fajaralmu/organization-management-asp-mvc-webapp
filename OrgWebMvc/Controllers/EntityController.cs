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
            ViewData["EntityType"] = typeof(division);
            ViewData["EntityList"] = BaseService.GetObjectList(DivisionSvc, Request);
            ViewData["Entity"] = "Division";

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
            ViewData["EntityType"] = typeof(program);
            ViewData["EntityList"] = BaseService.GetObjectList(ProgramSvc, Request);
            ViewData["Entity"] = "Program";
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
            ViewData["EntityType"] = typeof(@event);
            ViewData["EntityList"] = BaseService.GetObjectList(EventSvc, Request);
            ViewData["Entity"] = "Event";
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
            ViewData["EntityType"] = typeof(member);
            ViewData["EntityList"] = BaseService.GetObjectList(MemberSvc, Request);
            ViewData["Entity"] = "Member";
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
            ViewData["EntityType"] = typeof(position);
            ViewData["EntityList"] = BaseService.GetObjectList(PositionSvc, Request);
            ViewData["Entity"] = "Position";
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
            ViewData["EntityType"] = typeof(section);
            ViewData["EntityList"] = BaseService.GetObjectList(SectionSvc, Request);
            ViewData["Entity"] = "Section";
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
            ViewData["EntityType"] = typeof(post);
            ViewData["EntityList"] = BaseService.GetObjectList(PostSvc, Request);
            ViewData["Entity"] = "Post";
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
                    object Entity = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        Entity = EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    }
                    Response = new WebResponse(0, "Success", CustomHelper.GenerateFormString(typeof(division), Entity), EntitySvc.count);
                    break;
                case "Post":
                    division Division = (division)ObjectUtil.FillObjectWithMap(new division(), BaseService.ReqToDict(Request));
                    if (Division != null)
                    {
                        Division.user_id = LoggedUser.id;

                        division DBDivision = null;
                        string Info = "create";
                        if (Division.id != null && Division.id != 0)
                        {
                            Info = "update";
                            DBDivision = (division)EntitySvc.Update(Division);
                        }
                        else
                        {
                            DBDivision = (division)EntitySvc.Add(Division);
                        }
                        if (DBDivision == null)
                        {
                            return Json(Response);
                        }
                        division toSend = (division)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","description","user_id"
                        }, DBDivision);
                        Response = new WebResponse(0, "Success " + Info, toSend);
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    division DBDiv = (division)EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DBDiv != null)
                    {
                        EntitySvc.Delete(DBDiv);
                        Response = new WebResponse(0, "Success");
                    }
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
                    object Entity = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        Entity = EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    }
                    Response = new WebResponse(0, "Success", CustomHelper.GenerateFormString(typeof(program), Entity), EntitySvc.count);
                    break;
                case "Post":
                    program Program = (program)ObjectUtil.FillObjectWithMap(new program(), BaseService.ReqToDict(Request));
                    if (Program != null)
                    {
                        program ProgramDB = null;
                        string Info = "create";
                        if (Program.id != null && Program.id != 0)
                        {
                            Info = "update";
                            ProgramDB = (program)EntitySvc.Update(Program);
                        }
                        else
                        {
                            ProgramDB = (program)EntitySvc.Add(Program);
                        }
                        if (ProgramDB == null)
                        {
                            return Json(Response);
                        }
                        program toSend = (program)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","description","sect_id"
                        }, ProgramDB);
                        Response = new WebResponse(0, "Success " + Info, toSend);
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    program DBProg = (program)EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DBProg != null)
                    {
                        EntitySvc.Delete(DBProg);
                        Response = new WebResponse(0, "Success");
                    }
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
                    object Entity = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        Entity = EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    }
                    Response = new WebResponse(0, "Success", CustomHelper.GenerateFormString(typeof(@event), Entity), EntitySvc.count);
                    break;
                case "Post":
                    @event Event = (@event)ObjectUtil.FillObjectWithMap(new @event(), BaseService.ReqToDict(Request));
                    if (Event != null)
                    {
                        Event.user_id = LoggedUser.id;
                        @event EventDB = null;
                        string Info = "create";
                        if (Event.id != null && Event.id != 0)
                        {
                            Info = "update";
                            EventDB = (@event)EntitySvc.Update(Event);
                        }
                        else
                        {
                            EventDB = (@event)EntitySvc.Add(Event);
                        }
                        if (EventDB == null)
                        {
                            return Json(Response);
                        }
                        @event toSend = (@event)ObjectUtil.GetObjectValues(new string[]{
                            "id","program_id","user_id","date","location","participant","info","done","name"
                        }, EventDB);
                        Response = new WebResponse(0, "Success " + Info, toSend);
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    @event DbEvt = (@event)EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DbEvt != null)
                    {
                        EntitySvc.Delete(DbEvt);
                        Response = new WebResponse(0, "Success");
                    }
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
                    object Entity = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        Entity = EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    }
                    Response = new WebResponse(0, "Success", CustomHelper.GenerateFormString(typeof(member), Entity), EntitySvc.count);
                    break;
                case "Post":
                    member Member = (member)ObjectUtil.FillObjectWithMap(new member(), BaseService.ReqToDict(Request));
                    if (Member != null)
                    {
                        member MemberDB = null;
                        string Info = "create";
                        if (Member.id != null && Member.id != 0)
                        {
                            Info = "update";
                            MemberDB = (member)EntitySvc.Update(Member);
                        }
                        else
                        {
                            MemberDB = (member)EntitySvc.Add(Member);
                        }
                        if (MemberDB == null)
                        {
                            return Json(Response);
                        }
                        member toSend = (member)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","description","position_id"
                        }, MemberDB);
                        Response = new WebResponse(0, "Success " + Info, toSend);
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    member DBmmb = (member)EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DBmmb != null)
                    {
                        EntitySvc.Delete(DBmmb);
                        Response = new WebResponse(0, "Success");
                    }
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
                    object Entity = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        Entity = EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    }
                    Response = new WebResponse(0, "Success", CustomHelper.GenerateFormString(typeof(section), Entity), EntitySvc.count);
                    break;
                case "Post":
                    section section = (section)ObjectUtil.FillObjectWithMap(new section(), BaseService.ReqToDict(Request));
                    if (section != null)
                    {
                        section sectionDB = null;
                        string Info = "create";
                        if (section.id != null && section.id != 0)
                        {
                            Info = "update";
                            sectionDB = (section)EntitySvc.Update(section);
                        }
                        else
                        {
                            sectionDB = (section)EntitySvc.Add(section);
                        }
                        if (sectionDB == null)
                        {
                            return Json(Response);
                        }
                        section toSend = (section)ObjectUtil.GetObjectValues(new string[]{
                           "id","name","description","division_id","parent_section_id"
                        }, sectionDB);
                        Response = new WebResponse(0, "Success " + Info, toSend);
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    section DBsection = (section)EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DBsection != null)
                    {
                        EntitySvc.Delete(DBsection);
                        Response = new WebResponse(0, "Success");
                    }
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
                    object Entity = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        Entity = EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    }
                    Response = new WebResponse(0, "Success", CustomHelper.GenerateFormString(typeof(position), Entity), EntitySvc.count);
                    break;
                case "Post":
                    position position = (position)ObjectUtil.FillObjectWithMap(new position(), BaseService.ReqToDict(Request));
                    if (position != null)
                    {
                        position positionDB = null;
                        string Info = "create";
                        if (position.id != null && position.id != 0)
                        {
                            Info = "update";
                            positionDB = (position)EntitySvc.Update(position);
                        }
                        else
                        {
                            positionDB = (position)EntitySvc.Add(position);
                        }
                        if (positionDB == null)
                        {
                            return Json(Response);
                        }
                        position toSend = (position)ObjectUtil.GetObjectValues(new string[]{
                           "id","name","description","section_id","parent_position_id"
                        }, positionDB);
                        Response = new WebResponse(0, "Success " + Info, toSend);
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    position DBposition = (position)EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DBposition != null)
                    {
                        EntitySvc.Delete(DBposition);
                        Response = new WebResponse(0, "Success");
                    }
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
                        post Mmb = (post)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","user_id","title","body","date","type","post_id"
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
                        ResponseData = CustomHelper.GenerateDataTableString(typeof(post), ObjList);
                    }
                    Response = new WebResponse(0, "Success", ResponseData, EntitySvc.count);
                    break;
                case "Form":
                    object Entity = null;
                    if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        Entity = EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    }
                    Response = new WebResponse(0, "Success", CustomHelper.GenerateFormString(typeof(post), Entity), EntitySvc.count);
                    break;
                case "Post":
                    post post = (post)ObjectUtil.FillObjectWithMap(new post(), BaseService.ReqToDict(Request));
                    if (post != null)
                    {
                        post postDB = null;
                        string Info = "create";
                        post.user_id = LoggedUser.id;
                        if (post.id != null && post.id != 0)
                        {
                            Info = "update";
                            postDB = (post)EntitySvc.Update(post);
                        }
                        else
                        {
                            postDB = (post)EntitySvc.Add(post);
                        }
                        if (postDB == null)
                        {
                            return Json(Response);
                        }
                        post toSend = (post)ObjectUtil.GetObjectValues(new string[]{
                           "id","user_id","title","body","date","type","post_id"
                        }, postDB);
                        Response = new WebResponse(0, "Success " + Info, toSend);
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    post DBmmb = (post)EntitySvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DBmmb != null)
                    {
                        EntitySvc.Delete(DBmmb);
                        Response = new WebResponse(0, "Success");
                    }
                    break;
                default:
                    break;
            }
            return Json(Response);
        }


    }
}

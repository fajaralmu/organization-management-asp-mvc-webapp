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

            if (!base.UserValid())
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
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult Event()
        {
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult Member()
        {
            return View("~/Views/Shared/EntityMng.cshtml");
        }

        public ActionResult Post()
        {
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
            DivisionService DivisionSvc = new DivisionService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(DivisionSvc, Request);
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
                    Response.code = 0;
                    Response.message = "Success";
                    Response.data = CustomHelper.GenerateTableString(typeof(division), ObjList);
                    Response.count = DivisionSvc.count;
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
                            DBDivision = (division)DivisionSvc.Update(Division);
                        }
                        else
                        {
                            DBDivision = (division)DivisionSvc.Add(Division);
                        }
                        if (DBDivision == null)
                        {
                            return Json(Response);
                        }
                        division toSend = (division)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","description","user_id"
                        }, DBDivision);
                        Response.code = 0;
                        Response.message = "Success " + Info;
                        Response.data = toSend;
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    division DBDiv =(division) DivisionSvc.GetById(int.Parse(Request.Form["Id"]));
                    if(DBDiv != null)
                    {
                        DivisionSvc.Delete(DBDiv);
                        Response.code = 0;
                        Response.message = "Success";
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
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            ProgramService ProgramSvc = new ProgramService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(ProgramSvc, Request);
                    List<program> programs = (List<program>)ObjectUtil.ConvertList(ObjList, typeof(List<program>));
                    List<program> ListToSend = new List<program>();
                    foreach (program P in programs)
                    {
                        program Div = (program)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","name","description","division_id"
                        }, P);
                        ListToSend.Add(Div);
                    }
                    Response.code = 0;
                    Response.message = "Success";
                    Response.data = ListToSend;
                    Response.count = ProgramSvc.count;
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
                            ProgramDB = (program)ProgramSvc.Update(Program);
                        }
                        else
                        {
                            ProgramDB = (program)ProgramSvc.Add(Program);
                        }
                        if (ProgramDB == null)
                        {
                            return Json(Response);
                        }
                        program toSend = (program)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","description","division_id"
                        }, ProgramDB);
                        Response.code = 0;
                        Response.message = "Success " + Info;
                        Response.data = toSend;
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    program DBProg = (program)ProgramSvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DBProg != null)
                    {
                        ProgramSvc.Delete(DBProg);
                        Response.code = 0;
                        Response.message = "Success";
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
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            EventService EventSvc = new EventService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(EventSvc, Request);
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
                    Response.code = 0;
                    Response.message = "Success";
                    Response.data = ListToSend;
                    Response.count = EventSvc.count;
                    break;
                case "Post":
                    @event Event = (@event)ObjectUtil.FillObjectWithMap(new @event(), BaseService.ReqToDict(Request));
                    if (Event != null)
                    {
                        @event EventDB = null;
                        string Info = "create";
                        if (Event.id != null && Event.id != 0)
                        {
                            Info = "update";
                            EventDB = (@event)EventSvc.Update(Event);
                        }
                        else
                        {
                            EventDB = (@event)EventSvc.Add(Event);
                        }
                        if (EventDB == null)
                        {
                            return Json(Response);
                        }
                        @event toSend = (@event)ObjectUtil.GetObjectValues(new string[]{
                            "id","program_id","user_id","date","location","participant","info","done","name"
                        }, EventDB);
                        Response.code = 0;
                        Response.message = "Success " + Info;
                        Response.data = toSend;
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    @event DbEvt = (@event)EventSvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DbEvt != null)
                    {
                        EventSvc.Delete(DbEvt);
                        Response.code = 0;
                        Response.message = "Success";
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
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            MemberService MemberSvc = new MemberService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(MemberSvc, Request);
                    List<member> Members = (List<member>)ObjectUtil.ConvertList(ObjList, typeof(List<member>));
                    List<member> ListToSend = new List<member>();
                    foreach (member Obj in Members)
                    {
                        member Mmb = (member)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","name","position","division_id"
                        }, Obj);
                        ListToSend.Add(Mmb);
                    }
                    Response.code = 0;
                    Response.message = "Success";
                    Response.data = ListToSend;
                    Response.count = MemberSvc.count;
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
                            MemberDB = (member)MemberSvc.Update(Member);
                        }
                        else
                        {
                            MemberDB = (member)MemberSvc.Add(Member);
                        }
                        if (MemberDB == null)
                        {
                            return Json(Response);
                        }
                        member toSend = (member)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","position","division_id"
                        }, MemberDB);
                        Response.code = 0;
                        Response.message = "Success " + Info;
                        Response.data = toSend;
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    member DBmmb = (member)MemberSvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DBmmb != null)
                    {
                        MemberSvc.Delete(DBmmb);
                        Response.code = 0;
                        Response.message = "Success";
                    }
                    break;
                default:
                    break;
            }
            return Json(Response);
        }

        [HttpPost]
        public ActionResult postSvc()
        {
            WebResponse Response = new WebResponse();
            if (!StringUtil.NotNullAndNotBlank(Request.Form["Action"]))
            {
                return Json(Response);
            }
            string Action = Request.Form["Action"].ToString();
            PostService postSvc = new PostService();
            switch (Action)
            {
                case "List":
                    List<object> ObjList = BaseService.GetObjectList(postSvc, Request);
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
                    Response.code = 0;
                    Response.message = "Success";
                    Response.data = ListToSend;
                    Response.count = postSvc.count;
                    break;
                case "Post":
                    post post = (post)ObjectUtil.FillObjectWithMap(new post(), BaseService.ReqToDict(Request));
                    if (post != null)
                    {
                        post postDB = null;
                        string Info = "create";
                        if (post.id != null && post.id != 0)
                        {
                            Info = "update";
                            postDB = (post)postSvc.Update(post);
                        }
                        else
                        {
                            postDB = (post)postSvc.Add(post);
                        }
                        if (postDB == null)
                        {
                            return Json(Response);
                        }
                        post toSend = (post)ObjectUtil.GetObjectValues(new string[]{
                           "id","user_id","title","body","date","type","post_id"
                        }, postDB);
                        Response.code = 0;
                        Response.message = "Success " + Info;
                        Response.data = toSend;
                    }
                    break;
                case "Delete":
                    if (!StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
                    {
                        return Json(Response);
                    }
                    post DBmmb = (post)postSvc.GetById(int.Parse(Request.Form["Id"]));
                    if (DBmmb != null)
                    {
                        postSvc.Delete(DBmmb);
                        Response.code = 0;
                        Response.message = "Success";
                    }
                    break;
                default:
                    break;
            }
            return Json(Response);
        }


    }
}

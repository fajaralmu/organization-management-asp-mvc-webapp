using OrgWebMvc.Main.Service;
using OrgWebMvc.Main.Util;
using OrgWebMvc.Models;
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
            //SectionService SecSvc = new SectionService();
            //DivisionService DivSvc = new DivisionService();
            //foreach(object O in DivSvc.SearchAdvanced(new Dictionary<string, object>()))
            //{
            //    division D = (division)O;
            //    section Manajemen = new section()
            //    {
            //        division_id = D.id,
            //        name = "Manajemen " + D.name.Trim(),
            //        description = "management for "+D.name.Trim()

            //    };
            //    SecSvc.Add(Manajemen);
            //}

            return RedirectToAction("Index", "Home");
        }
        public ActionResult TimeLine()
        {
            //PositionService PosSvc = new PositionService();
            //SectionService SectionSvc = new SectionService();
            //List<object> Sections = SectionSvc.SearchAdvanced(new Dictionary<string, object>());

            //foreach (object P in Sections)
            //{
            //    section Section = (section)P;
            //    position Head = new position()
            //    {
            //        name = "Head " + Section.name.Replace("Manajemen", "").Trim(),
            //        description = "Head for " + Section.name.Replace("Manajemen", "").Trim(),
            //        section_id = Section.id
            //    };
            //    position Secretary = new position()
            //    {
            //        name = "Secretary " + Section.name.Replace("Manajemen", "").Trim(),
            //        description = "Secretary for " + Section.name.Replace("Manajemen", "").Trim(),
            //        section_id = Section.id
            //    };

            //    position newHead = (position)PosSvc.Add(Head);
            //    position Member = new position()
            //    {
            //        name = "Member " + Section.name.Replace("Manajemen", "").Trim(),
            //        description = "Member for " + Section.name.Replace("Manajemen", "").Trim(),
            //        section_id = Section.id,
            //        parent_position_id = newHead.id
            //    };
            //    PosSvc.Add(Secretary);
            //    PosSvc.Add(Member);
            //}

            if (!UserValid())
            {
                return RedirectToAction("Index", "Home");
            }
            /*
            MemberService MemberSvc = new MemberService();
            int pId = 0;
            for(int i = 0; i < Mock.Names.Length; i++)
            {
                if(pId >= Mock.PosId.Length)
                {
                    pId = 0;
                }
                member Member = new member()
                {
                    name = Mock.Names[i],
                    position_id = Mock.PosId[pId],
                    description = "MEMBER " + pId
                };
                MemberSvc.Add(Member);
                pId++;
            }

            for(int i = 0; i < Mock.PosMngId.Length; i++)
            {
                member Member = new member()
                {
                    name = Mock.MngNames[i],
                    position_id = Mock.PosMngId[i],
                    description = "MEMBER " + Mock.PosMngId[i]
                };
                MemberSvc.Add(Member);
            }
            */
            return View();
        }


    }
}
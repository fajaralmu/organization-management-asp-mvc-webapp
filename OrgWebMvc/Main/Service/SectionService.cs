﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrgWebMvc.Models;
using InstApp.Util.Common;

namespace OrgWebMvc.Main.Service
{
    public class SectionService

        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.sections orderby p.name select p);
            List<section> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (section c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.sections.Count();
            return ObjList;
        }
        public override object Update(object Obj)
        {
            Refresh();
            section section = (section)Obj;
            section DBsection = (section)GetById(section.id);
            if (DBsection == null)
            {
                return null;
            }
            dbEntities.Entry(DBsection).CurrentValues.SetValues(section);
            try
            {
                dbEntities.SaveChanges();
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return section;
        }

        public override object GetById(object Id)
        {
            section section = (from c in dbEntities.sections where c.id == (int)Id select c).SingleOrDefault();
            return section;
        }

        public override void Delete(object Obj)
        {
            section section = (section)Obj;
            dbEntities.sections.Remove(section);
            dbEntities.SaveChanges();
        }




        public override int ObjectCount()
        {
            return count;// dbEntities.sections.Count();
        }

        public override object Add(object Obj)
        {
            section section = (section)Obj;
            section newsection = dbEntities.sections.Add(section);
            try
            {
                dbEntities.SaveChanges();
                return newsection;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                //  return null;
            }

        }

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            List<object> categoryList = new List<object>();
            var sections = dbEntities.sections
                .SqlQuery(sql
                ).
                Select(section => new
                {
                    section
                });
            if (limit > 0)
            {
                sections = sections.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                sections = sections.ToList();
            }
            foreach (var u in sections)
            {
                section section = u.section;
                categoryList.Add(section);
            }

            return categoryList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {

            string id = Params.ContainsKey("id") ? Params["id"].ToString() : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string division = Params.ContainsKey("division") ? (string)Params["division"] : "";
            string user_id = Params.ContainsKey("user_id") ? Params["user_id"].ToString() : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from section " +
          //      " left join section on section.id = section.section_id " +
         //        " left join section on section.id = section.section_id " +
                " left join division on division.id = section.division_id " +
                " where section.id like '%" + id + "%'" +
                " and section.name like '%" + name + "%' " +
                " and division.name like '%" + division + "%' " +
                (StringUtil.NotNullAndNotBlank(user_id) ? " and division.user_id=" + user_id : "");
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.sections);
            return ListWithSql(sql, limit, offset);
        }


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<section>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}
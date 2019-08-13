using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrgWebMvc.Models;
using InstApp.Util.Common;

namespace OrgWebMvc.Main.Service
{
    public class ProgramService
        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.programs orderby p.name select p);
            List<program> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (program c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.programs.Count();
            return ObjList;
        }
        public override object Update(object Obj)
        {
            Refresh();
            program program = (program)Obj;
            program DBProgram = (program)GetById(program.id);
            if (DBProgram == null)
            {
                return null;
            }
            dbEntities.Entry(DBProgram).CurrentValues.SetValues(program);
            dbEntities.SaveChanges();
            return program;
        }

        public override object GetById(object Id)
        {
            program program = (from c in dbEntities.programs where c.id == (int)Id select c).SingleOrDefault();
            return program;
        }

        public override void Delete(object Obj)
        {
            program program = (program)Obj;
            dbEntities.programs.Remove(program);
            dbEntities.SaveChanges();
        }




        public override int ObjectCount()
        {
            return count;// dbEntities.programs.Count();
        }

        public override object Add(object Obj)
        {
            program program = (program)Obj;

            program newProgram = dbEntities.programs.Add(program);
            try
            {
                dbEntities.SaveChanges();
                return newProgram;
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
            var programs = dbEntities.programs
                .SqlQuery(sql
                ).
                Select(program => new
                {
                    program
                });
            if (limit > 0)
            {
                programs = programs.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                programs = programs.ToList();
            }
            foreach (var u in programs)
            {
                program program = u.program;
                categoryList.Add(program);
            }

            return categoryList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {

            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string user_id = Params.ContainsKey("user_id") ? Params["user_id"].ToString() : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string section = Params.ContainsKey("section") ? (string)Params["section"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from program "+
                " left join section on section.id = program.sect_id "+
                " left join division on division.id = section.division_id " +
                " where program.id like '%" + id + "%'" +
                " and program.name like '%" + name + "%'" +
                " and section.name like '%" + section + "%'" +
                 (StringUtil.NotNullAndNotBlank(user_id) ? " and division.user_id=" + user_id : "");
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.programs);
            return ListWithSql(sql, limit, offset);
        }


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<program>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrgWebMvc.Models;
using InstApp.Util.Common;

namespace OrgWebMvc.Main.Service
{
    public class PositionService

        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.positions orderby p.name select p);
            List<position> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (position c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.positions.Count();
            return ObjList;
        }
        public override object Update(object Obj)
        {
            Refresh();
            position position = (position)Obj;
            position DBposition = (position)GetById(position.id);
            if (DBposition == null)
            {
                return null;
            }
            dbEntities.Entry(DBposition).CurrentValues.SetValues(position);
            dbEntities.SaveChanges();
            return position;
        }

        public override object GetById(object Id)
        {
            position position = (from c in dbEntities.positions where c.id == (int)Id select c).SingleOrDefault();
            return position;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                position position = (position)Obj;
                dbEntities.positions.Remove(position);
                dbEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }




        public override int ObjectCount()
        {
            return count;// dbEntities.positions.Count();
        }

        public override object Add(object Obj)
        {
            position position = (position)Obj;
            position newposition = dbEntities.positions.Add(position);
            try
            {
                dbEntities.SaveChanges();
                return newposition;
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
            var positions = dbEntities.positions
                .SqlQuery(sql
                ).
                Select(position => new
                {
                    position
                });
            if (limit > 0)
            {
                positions = positions.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                positions = positions.ToList();
            }
            foreach (var u in positions)
            {
                position position = u.position;
                categoryList.Add(position);
            }

            return categoryList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {

            string id = Params.ContainsKey("id") ? Params["id"].ToString() : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string section = Params.ContainsKey("section") ? (string)Params["section"] : "";
            string user_id = Params.ContainsKey("user_id") ? Params["user_id"].ToString() : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from position " +
                 // " left join position on position.id = position.parent_position_id " +
                 " left join section on section.id = position.section_id " +
                " left join division on division.id = section.division_id " +
                " where position.id like '%" + id + "%' " +
                " and position.name like '%" + name + "%' " +
                " and section.name like '%" + section + "%' " +
                (StringUtil.NotNullAndNotBlank(user_id) ? " and division.user_id=" + user_id : "");
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.positions);
            return ListWithSql(sql, limit, offset);
        }


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<position>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}
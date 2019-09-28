using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrgWebMvc.Models;
using InstApp.Util.Common;

namespace OrgWebMvc.Main.Service
{
    public class MemberService

        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            dbEntities = new ORG_DBEntities();
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.members orderby p.name select p);
            List<member> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (member c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.members.Count();
            return ObjList;
        }
        public override object Update(object Obj)
        {
            dbEntities = new ORG_DBEntities();
            member member = (member)Obj;
            member DBmember = (member)GetById(member.id);
            if (DBmember == null)
            {
                return null;
            }
            dbEntities.Entry(DBmember).CurrentValues.SetValues(member);
            dbEntities.SaveChanges();
            return member;
        }

        public override object GetById(object Id)
        {
            dbEntities = new ORG_DBEntities();
            member member = (from c in dbEntities.members where c.id == (int)Id select c).SingleOrDefault();
            return member;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                dbEntities = new ORG_DBEntities();
                member member = (member)Obj;
                dbEntities.members.Remove(member);
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
            return count;// dbEntities.members.Count();
        }

        public override object Add(object Obj)
        {
            member member = (member)Obj;
            try
            {
                dbEntities = new ORG_DBEntities();
                member newmember = dbEntities.members.Add(member);

                dbEntities.SaveChanges();
                return newmember;
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
            dbEntities = new ORG_DBEntities();

            var members = dbEntities.members
                .SqlQuery(sql
                ).
                Select(member => new
                {
                    member
                });
            if (limit > 0)
            {
                members = members.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                members = members.ToList();
            }
            foreach (var u in members)
            {
                member member = u.member;
                categoryList.Add(member);
            }

            return categoryList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0, bool updateCount = true)
        {

            string id = Params.ContainsKey("id") ? Params["id"].ToString() : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string position = Params.ContainsKey("position") ? (string)Params["position"] : "";
            string institution_id = Params.ContainsKey("institution_id") ? Params["institution_id"].ToString() : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from [member] " +
                " left join [position] on [position].[id] = [member].[position_id] " +
                 " left join [section] on [section].[id] = [position].[section_id] " +
                " left join [division] on [division].[id] = [section].[division_id] " +
                " where [member].[id] like '%" + id + "%'" +
                " and [member].[name] like '%" + name + "%' " +
                " and [position].[name] like '%" + position + "%' " +
                (StringUtil.NotNullAndNotBlank(institution_id) ? " and [division].[institution_id] =" + institution_id : "");
            sql += StringUtil.AddSortQuery(orderby, ordertype);
            dbEntities = new ORG_DBEntities();
            count = countSQL(sql, dbEntities.members);
            return ListWithSql(sql, limit, offset);
        }


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<member>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}
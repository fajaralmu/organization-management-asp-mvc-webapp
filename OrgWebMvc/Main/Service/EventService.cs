using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrgWebMvc.Models;

namespace OrgWebMvc.Main.Service
{
    public class EventService
        : BaseService
    {
        
        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.events orderby p.name select p);
            List<@event> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (@event c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.events.Count();
            return ObjList;
        }
        public override object Update(object Obj)
        {
            Refresh();
            @event @event = (@event)Obj;
            @event DBEvent = (@event)GetById(@event.id);
            if (DBEvent == null)
            {
                return null;
            }
            dbEntities.Entry(DBEvent).CurrentValues.SetValues(@event);
            dbEntities.SaveChanges();
            return @event;
        }

        public override object GetById(object Id)
        {
            @event @event = (from c in dbEntities.events where c.id.Equals(Id) select c).SingleOrDefault();
            return @event;
        }

        public override void Delete(object Obj)
        {
            @event @event = (@event)Obj;
            dbEntities.events.Remove(@event);
            dbEntities.SaveChanges();
        }


        

        public override int ObjectCount()
        {
            return count;// dbEntities.events.Count();
        }

        public override object Add(object Obj)
        {
            @event @event = (@event)Obj;
           
            @event newEvent = dbEntities.events.Add(@event);
            try
            {
                dbEntities.SaveChanges();
                return newEvent;
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
            var Events = dbEntities.events
                .SqlQuery(sql
                ).
                Select(@event => new
                {
                    @event
                });
            if (limit > 0)
            {
                Events = Events.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                Events = Events.ToList();
            }
            foreach (var u in Events)
            {
                @event @event = u.@event;
                categoryList.Add(@event);
            }

            return categoryList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {

            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from @event where id like '%" + id + "%'" +
                " and name like '%" + name + "%'";
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.events);
            return ListWithSql(sql, limit, offset);
        }


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<@event>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}
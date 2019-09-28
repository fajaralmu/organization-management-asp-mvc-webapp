using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrgWebMvc.Models;
using InstApp.Util.Common;

namespace OrgWebMvc.Main.Service
{
    public class EventService
        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            dbEntities = new ORG_DBEntities();
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
            dbEntities = new ORG_DBEntities();
            @event @event = (@event)Obj;
            @event.created_date = DateTime.Now;
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
            dbEntities = new ORG_DBEntities();
            @event @event = (from o in dbEntities.events where o.id == (int)Id select o).SingleOrDefault();
            return @event;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                dbEntities = new ORG_DBEntities();
                @event @event = (@event)Obj;
                dbEntities.events.Remove(@event);
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
            return count;// dbEntities.events.Count();
        }

        public override object Add(object Obj)
        {
            @event @event = (@event)Obj;
            dbEntities = new ORG_DBEntities();
            @event.created_date = DateTime
                .Now;
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
            dbEntities = new ORG_DBEntities();
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

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0, bool updateCount = true)
        {

            string id = Params.ContainsKey("id") ? Params["id"].ToString() : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string program = Params.ContainsKey("program") ? (string)Params["program"] : "";
            string location = Params.ContainsKey("location") ? (string)Params["location"] : "";
            string participant = Params.ContainsKey("participant") ? Params["participant"].ToString() : "";
            string info = Params.ContainsKey("info") ? (string)Params["info"] : "";

           
            string institution_id = Params.ContainsKey("institution_id") ? Params["institution_id"].ToString() : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string dateFilterQuery = StringUtil.AddDateFilterQuery(Params, "event", "date", true);
            string sql = "select * from [event] left join [program] on [program].[id]=[event].[program_id] " +
                " left join [section] on [section].[id] = [program].[sect_id] " +
                " left join [division] on [division].[id] = [section].[division_id] where [event].[id] like '%" + id + "%'" +
                " and [event].[name] like '%" + name + "%' " +
                " and [program].[name]  like '%" + program + "%' " +
                " and [event].[location]  like '%" + location + "%' " +
                " and [event].[participant]  like '%" + participant + "%' " +
                " and [event].[info]  like '%" + info + "%' " +
                (StringUtil.NotNullAndNotBlank(institution_id) ? " and [division].[institution_id] = " + institution_id : "") +
                dateFilterQuery;
            /* (StringUtil.NotNullAndNotBlank(day) ? " and DAY([event].[date]) = " + day : "") +
                (StringUtil.NotNullAndNotBlank(month) ? " and MONTH([event].[date]) = " + month : "") +
                (StringUtil.NotNullAndNotBlank(year) ? " and YEAR([event].[date]) = " + year : "");
          */
            sql += StringUtil.AddSortQuery(orderby, ordertype);
            dbEntities = new ORG_DBEntities();
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
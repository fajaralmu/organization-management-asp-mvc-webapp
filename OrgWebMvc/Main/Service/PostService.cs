using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrgWebMvc.Models;
using InstApp.Util.Common;
using System.Threading.Tasks;

namespace OrgWebMvc.Main.Service
{
    public class PostService
        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            dbEntities = new ORG_DBEntities();
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.posts orderby p.title select p);
            List<post> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (post c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.posts.Count();
            return ObjList;
        }
        public override object Update(object Obj)
        {
            dbEntities = new ORG_DBEntities();
            post post = (post)Obj;
            post DBpost = (post)GetById(post.id);
            if (DBpost == null)
            {
                return null;
            }
            dbEntities.Entry(DBpost).CurrentValues.SetValues(post);
            dbEntities.SaveChanges();
            return post;
        }

        public override object GetById(object Id)
        {
            dbEntities = new ORG_DBEntities();
            post post = (from c in dbEntities.posts where c.id == (int)Id select c).SingleOrDefault();
            return post;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                post post = (post)Obj;
                dbEntities = new ORG_DBEntities();
                dbEntities.posts.Remove(post);
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
            return count;// dbEntities.posts.Count();
        }

        public override object Add(object Obj)
        {
            post post = (post)Obj;

            dbEntities = new ORG_DBEntities();
            post.created_date = DateTime.Now;
            post newpost = dbEntities.posts.Add(post);
            try
            {
                dbEntities.SaveChanges();
                return newpost;
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

        public post findLatestPost()
        {
            dbEntities = new ORG_DBEntities();
            Dictionary<string, object> filter = new Dictionary<string, object>();
            filter.Add("orderby", "post.created_date");
            filter.Add("ordertype", "DESC");
            List<object> posts = SearchAdvanced(filter, 1, 0 ,false);
            if (posts == null || posts.Count == 0)
            {
                return null;
            }
            post Post = (post)posts[0];//.SingleOrDefault();
                                       //    return postObj.SingleOrDefault();
            return Post;
        }

        private async Task<List<object>> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            List<object> categoryList = new List<object>();
            dbEntities = new ORG_DBEntities();
            var posts = dbEntities.posts
                .SqlQuery(sql
                ).
                Select(post => new
                {
                    post
                });
            if (posts != null)
            {
                if (limit > 0)
                {
                    posts = posts.Skip(offset * limit).Take(limit);
                    if(posts == null)
                    {

                        return new List<object>();
                    }
                    posts = posts.ToList();
                }
                else posts = posts.ToList();

                foreach (var u in posts)
                {
                    post post = u.post;
                    categoryList.Add(post);
                }

                return categoryList;
            }

            return new List<object>();

        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0, bool updateCount = true)
        {

            string id = Params.ContainsKey("id") ? Params["id"].ToString() : "";
            string user_id = Params.ContainsKey("user_id") ? Params["user_id"].ToString() : "";
            string title = Params.ContainsKey("title") ? (string)Params["title"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";
            string institution_id = Params.ContainsKey("institution_id") ? Params["institution_id"].ToString() : "";

            string dateFilterQuery = StringUtil.AddDateFilterQuery(Params, "post", "date", true);

            string sql = "select * from [post] left join [user] on [user].[id] = [post].[user_id] where [post].[id] like '%" + id + "%'" +
                " and [post].[title] like '%" + title + "%' " +
                 (StringUtil.NotNullAndNotBlank(user_id) ? " and [post].[user_id]=" + user_id : "") +
                  (StringUtil.NotNullAndNotBlank(institution_id) ? " and [user].[institution_id]=" + institution_id : "") +
                 dateFilterQuery;
            sql += StringUtil.AddSortQuery(orderby, ordertype);
            dbEntities = new ORG_DBEntities();
           if(updateCount) count = countSQL(sql, dbEntities.posts);
            Task<List<object>> task =  ListWithSql(sql, limit, offset);
            if(task != null && !task.IsFaulted && task.IsCompleted )
            {
                return task.Result;
            }
            return new List<object>();
        }


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<post>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}
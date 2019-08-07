using InstApp.Util.Common;
using OrgWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgWebMvc.Main.Service
{
    public class BaseService
    {
        public int count = 0;
        protected static INST_DBEntities dbEntities;

        public BaseService()
        {
            Refresh();
        }

        public virtual List<object> ObjectList(int offset, int limit)
        {
            return null;
        }

        public virtual object Update(object obj)
        {
            return null;
        }

        public virtual object GetById(object Id)
        {
            return null;
        }

        public virtual void Delete(object obj)
        {

        }

        public virtual int ObjectCount()
        {
            return count;
        }

        public virtual object Add(object Obj)
        {
            return null;
        }

        public virtual List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            return null;
        }

        public virtual int countSQL(string sql, object dbSet)
        {
            return count;
        }

        public virtual int getCountSearch()
        {
            return count;
        }

        public static List<object> GetObjectList( BaseService Service, HttpRequestBase Req)
        {
            int Offset = 0;
            int Limit = 0;
            Dictionary<string, object> Params = new Dictionary<string, object>();
            if (StringUtil.NotNullAndNotBlank(Req.Form["limit"]) && StringUtil.NotNullAndNotBlank(Req.Form["offset"]))
            {
                try
                {
                    Offset = int.Parse(Req.Form["offset"].ToString());
                    Limit = int.Parse(Req.Form["limit"].ToString());

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            if (StringUtil.NotNullAndNotBlank(Req.Form["search_param"]))
            {
                string Param = Req.Form["search_param"].ToString();
                Param = Param.Replace("${", "");
                Param = Param.Replace("}$", "");
                Param = Param.Replace(";", "&");
                Params = StringUtil.QUeryStringToDict(Param);
            }
            return Service.SearchAdvanced(Params, Limit, Offset);

        }

        public static Dictionary<string, object> ReqToDict(HttpRequestBase Req)
        {
            if (StringUtil.NotNullAndNotBlank(Req.Form["field_param"]))
            {
                string Param = Req.Form["field_param"].ToString();
                Param = Param.Replace("${", "");
                Param = Param.Replace("}$", "");
                Param = Param.Replace(";", "&");
                return StringUtil.QUeryStringToDict(Param);
            }
            return null;
        }

        protected void Refresh()
        {
            if (dbEntities != null)
                dbEntities.Dispose();
            dbEntities = INST_DBEntities.Instance();

        }

    }
}
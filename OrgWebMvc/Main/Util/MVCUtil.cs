using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrgWebMvc.Main.API;
using OrgWebMvc.Main.Service;
using InstApp.Util.Common;
using OrgWebMvc.Models;

namespace OrgWebMvc.Main.Util
{
    public class MVCUtil
    {
        internal static ViewDataDictionary PopulateCRUDViewData(Type type, string Title, BaseService Svc, HttpRequestBase request, ViewDataDictionary ViewData)
        {
            ViewData["EntityType"] = type;
            ViewData["EntityList"] = BaseService.GetObjectList(Svc, request);
            ViewData["Entity"] = Title;
            return ViewData;
        }

        internal static WebResponse generateResponseWithForm(Type type, BaseService entitySvc, HttpRequestBase request)
        {
            object Entity = null;
            if (StringUtil.NotNullAndNotBlank(request.Form["Id"]))
            {
                Entity = entitySvc.GetById(int.Parse(request.Form["Id"]));
            }
            return new WebResponse(0, "Success", CustomHelper.GenerateFormString(type, Entity), entitySvc.count); 
        }


        internal static WebResponse DeleteEntity(BaseService entitySvc, HttpRequestBase request, WebResponse response)
        {
            if (!StringUtil.NotNullAndNotBlank(request.Form["Id"]))
            {
                return (response);
            }
            object DBposition = entitySvc.GetById(int.Parse(request.Form["Id"]));
            if (DBposition != null)
            {
                if (entitySvc.Delete(DBposition))
                    return new WebResponse(0, "Success");
                else
                    return (response);
            }else
            {
                return (response);
            }
        }

        internal static WebResponse UpdateEntity(BaseService EntitySvc, object Entity, string[] objParamToSend, WebResponse response)
        {
            object DBObject = null;
            string Info = "create";
            object IDVal = ObjectUtil.GetIDVal(Entity);
            if (IDVal != null && IDVal.ToString() !="" && IDVal.ToString() != "0")
            {
                Info = "update";
                DBObject = EntitySvc.Update(Entity);
            }
            else
            {
                DBObject = EntitySvc.Add(Entity);
            }
            if (DBObject == null)
            {
                return (response);
            }

            object toSend = ObjectUtil.GetObjectValues(objParamToSend, DBObject);
            return new WebResponse(0, "Success " + Info, toSend);
        }
    }
}
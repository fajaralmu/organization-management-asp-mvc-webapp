using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OurLibrary.Util.Common
{
    public class JSONUtil
    {

        public static string MapToJsonString(Dictionary<string, object> Map)
        {
            string json = "";
            json += "{";
            int i = 0;
            int size = Map.Keys.Count;
            foreach (var key in Map.Keys)
            {
                i++;
                object Value = Map[key];
                if (null != Value && Value.GetType() == typeof(Dictionary<string, object>))
                {
                    Dictionary<string, object> MapVal = (Dictionary<string, object>)Value;
                    Value = MapToJsonString(MapVal);
                }
                else
                {
                    Value = "\"" + Value + "\"";
                }
                json += "\"" + key + "\":" + Value;
                if (i < size)
                {
                    json += ",";
                }
            }
            json += "}";
            return json;
        }
        static bool IsSimple(Type type)
        {
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        public static int rec = 0;

        public static string ObjToJsonString(object Object)
        {

            string json = "";
            json += "{";
            PropertyInfo[] Props = Object.GetType().GetProperties();
            int size = Props.Count();
            for (int i = 0; i < size; i++)
            {
                PropertyInfo prop = Props[i];
                object Value = "null-init-";
                try
                {
                    Value = prop.GetValue(Object);
                }
                catch (Exception e)
                {
                    Value = "null-error-";
                }
                if (null != Value && !IsSimple(Value.GetType()) && !Value.GetType().IsArray)
                {
                    object ObjVal = Value;
                    try
                    {
                        rec++;
                        if (rec >= 1)
                        {
                            rec = 0;
                            Value = "\"" + ObjVal + " more..\"";
                        }
                        else
                        {
                            Value = ObjToJsonString(ObjVal);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error parse:" + e.StackTrace);
                        Value = "\"" + Value + "\"";
                    }
                }
                else
                {
                    Value = "\"" + (Value==null?null:Value.ToString().Replace("\\","\\\\")) + "\"";
                }
                json += "\"" + prop.Name + "\":" + Value;
                if (i < size - 1)
                {
                    json += ",";
                }
            }
            json += "}";
            return json;
        }
    }
}
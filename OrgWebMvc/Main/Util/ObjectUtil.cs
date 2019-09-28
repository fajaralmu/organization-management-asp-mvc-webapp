
using InstApp.Annotation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace InstApp.Util.Common
{
    public class ObjectUtil
    {
        public static string CreateInsertQuery(string Name, string[] Params, object Object)
        {
            string Q = "INSERT INTO   " + Name + "  (";
            string Val = "";
            for (int i = 0; i < Params.Length; i++)
            {
                string PropertyName = Params[i];
                if (!HasProperty(PropertyName, Object))
                    continue;
                Q += PropertyName;
                object Value = GetValueFromProp(PropertyName, Object);
                if (Value.GetType().Equals(typeof(DateTime)))
                {
                    Value = StringUtil.DateTimeToString((DateTime)Value);
                }

                Val += "'" + Value + "'";

                if (i < Params.Length - 1)
                {
                    Q += ",";
                    Val += ",";
                }


            }
            Q = Q + ")VALUES(" + Val + ")";

            return Q;

        }

        public static object ConvertList(List<object> value, Type type)
        {
            IList list = (IList)Activator.CreateInstance(type);
            foreach (var item in value)
            {
                list.Add(item);
            }
            return list;
        }

        public static object GetValueOfArray(object Value, object[] Values, object[] Keys)
        {
            if (Values.Length > Keys.Length)
            {
                return null;
            }
            for (int i = 0; i < Values.Length; i++)
            {
                if (Value.Equals(Values[i]))
                {
                    return Keys[i];
                }
            }
            return null;
        }

        public static object GetValueFromProp(string propname, object Object)
        {
            if (Object == null)
            {
                return null;
            }
            return Object.GetType().GetProperty(propname).GetValue(Object);
        }



        public static object GetIDVal(object Object)
        {
            string IDField = GetIDProps(Object.GetType());
            if (IDField != null)
            {
                return GetValueFromProp(IDField, Object);
            }
            return null;
        }

        public static string GetIDProps(Type t)
        {
            //Type t = Type.GetType(ObjectPath);
            PropertyInfo[] Props = t.GetProperties();
            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo PropsInfo = Props[i];
                object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    if (Attribute.FieldType.Equals(AttributeConstant.TYPE_ID_AI) ||
                       Attribute.FieldType.Equals(AttributeConstant.TYPE_ID_NUMB) ||
                       Attribute.FieldType.Equals(AttributeConstant.TYPE_ID_STR_NUMB))

                    {
                        return PropsInfo.Name;
                    }
                }
            }
            return null;
        }

        public static object GetObjectValues(string[] Props, object OriginalObj)
        {
            object NewObject = Activator.CreateInstance(OriginalObj.GetType());
            for (int i = 0; i < Props.Length; i++)
            {
                string PropName = Props[i];
                if (HasProperty(PropName, OriginalObj))
                {
                    object val = OriginalObj.GetType().GetProperty(PropName).GetValue(OriginalObj);
                    NewObject.GetType().GetProperty(PropName).SetValue(NewObject, val);
                }
            }
            return NewObject;
        }

        public static object GetObjectValues(Dictionary<string, object> Props, object OriginalObj)
        {
            object NewObject = Activator.CreateInstance(OriginalObj.GetType());

            foreach (string Key in Props.Keys)
            {
                string PropName = Key;
                object PropVal = Props[Key];

                if (HasProperty(PropName, OriginalObj))
                {
                    object val = null;
                    val = OriginalObj.GetType().GetProperty(PropName).GetValue(OriginalObj);
                    if (val != null && PropVal!=null && PropVal.GetType().Equals(typeof(Dictionary<string, object>)))
                    {
                        val = GetObjectValues((Dictionary<string, object>)PropVal, val);
                    }
                    NewObject.GetType().GetProperty(PropName).SetValue(NewObject, val);
                }
            }

            return NewObject;
        }

        public static bool HasProperty(string PropName, object O)
        {
            foreach (PropertyInfo Prop in O.GetType().GetProperties())
            {
                if (Prop.Name.Equals(PropName))
                {
                    return true;
                }
            }
            return false;
        }

        public static string[] ValidateProps(string[] Props, object Obj)
        {
            List<string> ValidProp = new List<string>();
            for (int j = 0; j < Props.Length; j++)
            {
                string Prop = Props[j];
                if (HasProperty(Prop, Obj))
                {
                    ValidProp.Add(Prop);
                }
            }
            return ValidProp.ToArray<string>();
        }

        public static List<object> ICollectionToListObj(ICollection Collection)
        {
            List<object> List = new List<object>();
            foreach (var item in Collection)
            {
                if (item == null)
                    continue;
                List.Add(item);
            }
            return List;
        }

        public static object FillObjectWithMap(object OBJ, Dictionary<string, object> ObjMap)
        {
            if (ObjMap == null)
            {
                return null;
            }
            foreach (string key in ObjMap.Keys)
            {
                object keyVal = ObjMap[key];
                if (null != keyVal && HasProperty(key, OBJ))
                {
                    PropertyInfo PropInfo = OBJ.GetType().GetProperty(key);

                    object Value = null;
                    Type KeyType = keyVal.GetType();



                    if (KeyType.Equals(typeof(Int64)))
                    {
                        Value = Convert.ToInt32(ObjMap[key]);
                    }
                    else
                    {
                        Value = ObjMap[key];
                    }
                    if (null == Value)
                    {
                        continue;
                    }
                    if (Value.GetType().Equals(typeof(Dictionary<string, object>)))
                    {
                        object ObjValue = Activator.CreateInstance(OBJ.GetType().GetProperty(key).PropertyType);
                        Value = FillObjectWithMap(ObjValue, (Dictionary<string, object>)Value);
                    }
                    else if (Value.GetType().Equals(typeof(List<>)) || Value.GetType().Equals(typeof(ICollection)))
                    {
                        List<object> ObjList = new List<object>();
                        List<object> ValList = (List<object>)Value;
                        foreach (object o in ValList)
                        {
                            if (o.GetType().Equals(typeof(Dictionary<string, object>)))
                            {
                                object itemVal = null;
                                object ObjValue = Activator.CreateInstance(OBJ.GetType().GetProperty(key).PropertyType);
                                itemVal = FillObjectWithMap(null, (Dictionary<string, object>)o);
                                ObjList.Add(itemVal);
                            }
                        }
                        Value = ObjList;
                    }
                    if (Value == null || Value.ToString() == "")
                    {
                        continue;
                    }
                    if (PropInfo.PropertyType.Equals(typeof(System.Int32)))
                    {
                        Value = int.Parse(Value.ToString());
                    }
                    else if (PropInfo.PropertyType.Equals(typeof(Nullable<System.Int32>)))
                    {
                        int IntVal = int.Parse(Value.ToString());
                        Value = new Nullable<int>(IntVal);

                    }
                    else if (PropInfo.PropertyType.Equals(typeof(System.DateTime)))
                    {
                        string DateStr = Value.ToString();
                        DateTime Date = DateTime.Now;
                        DateTime.TryParse(DateStr, out Date);
                        Value = Date;
                    }

                    OBJ.GetType().GetProperty(key).SetValue(OBJ, Value);
                }
            }
            return OBJ;
        }


        public static string ListToDelimitedString(object ListOfObject, string Delimiter, string ValDelimiter, params string[] Props)
        {
            String ListString = "";
            if (ListOfObject == null)
                return "";
            List<object> List = ICollectionToListObj((ICollection)ListOfObject);
            if (List == null || List.Count == 0)
            {
                return "";
            }
            string[] ValidProps = ValidateProps(Props, List.ElementAt(0));

            for (int i = 0; i < List.Count; i++)

            {
                object Obj = List.ElementAt(i);
                string Value = "";
                for (int j = 0; j < ValidProps.Length; j++)
                {
                    string Prop = ValidProps[j];
                    Value += GetValueFromProp(Prop, Obj).ToString();
                    if (j < ValidProps.Length - 1)
                    {
                        Value += ValDelimiter;
                    }
                }
                ListString += Value;
                if (i < List.Count - 1)
                {
                    ListString += Delimiter;
                }

            }
            return ListString;
        }

        public static bool IsDisposed(DbContext context)
        {
            if (context == null) return false;
            var result = true;

            var typeDbContext = typeof(DbContext);
            var typeInternalContext = typeDbContext.Assembly.GetType("System.Data.Entity.Internal.InternalContext");

            var fi_InternalContext = typeDbContext.GetField("_internalContext", BindingFlags.NonPublic | BindingFlags.Instance);
            var pi_IsDisposed = typeInternalContext.GetProperty("IsDisposed");

            var ic = fi_InternalContext.GetValue(context);

            if (ic != null)
            {
                result = (bool)pi_IsDisposed.GetValue(ic);
            }

            return result;
        }

    }


}
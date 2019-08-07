using InstApp.Annotation;
using InstApp.Util.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OrgWebMvc.Main.Util
{
    public class CustomHelper
    {
        public static IHtmlString GenerateTable(Type ObjectType, List<object> Collection)
        {
            return new HtmlString(GenerateTableString(ObjectType, Collection));
        }

        public static string GenerateTableString(Type ObjectType, List<object> Collection)
        {
            HtmlTag Table = new HtmlTag("table");
            Table.Class = ("table");
            Table.ID = "table-list-entity";

            HtmlTag ColumnNames = new HtmlTag("tr");
            HtmlTag No = new HtmlTag("th", "#");
            No.AddAttribute("scope", "col");
            ColumnNames.Add(No);

            List<PropertyInfo> CustomedProp = new List<PropertyInfo>();

            PropertyInfo[] Props = ObjectType.GetProperties();
            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo Prop = Props[i];
                object[] Attributes = Prop.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    HtmlTag Th = new HtmlTag("th", Prop.Name.ToUpper());
                    ColumnNames.Add(Th);
                    CustomedProp.Add(Prop);
                }
            }
            ColumnNames.Add(new HtmlTag("th", "Option"));
            HtmlTag THead = new HtmlTag("thead", ColumnNames);
            HtmlTag TBody = new HtmlTag("tbody");

            THead.Class = ("thead-dark");

            Table.Add(THead);
            int Idx = 1;


            foreach (object Obj in Collection)
            {
                HtmlTag Tr = new HtmlTag("tr");
                Tr.ID = "row-obj-" + Idx;
                Tr.Add(new HtmlTag("td", Idx.ToString()));
                foreach (PropertyInfo Prop in CustomedProp)
                {
                    HtmlTag Td = new HtmlTag("td", ObjectType.GetProperty(Prop.Name).GetValue(Obj).ToString());
                    Tr.Add(Td);
                }
                Idx++;
                Tr.Add(new HtmlTag("td", "buttons"));
                TBody.Add(Tr);
            }

            Table.Add(TBody);
            //   string LableStr = $ "<label style=\"background-color:gray;color:yellow;font-size:24px\">{Content}</label>";
            return ControlUtil.HtmlTagToString(Table);
        }

        public static IHtmlString GenerateForm(Type ObjectType)
        {

            string ObjName = StringUtil.ToUpperCase(0, ObjectType.Name);
            HtmlTag Form = new HtmlTag("form");
            Form.ID = "form-entity";
            Form.Class = "form";
            //  Form.AddAttribute("method", "POST");
            //  Form.AddAttribute("action", ObjName + "Svc");

            Form.AddAttribute("onsubmit", "return submitEvent(event)");

            PropertyInfo[] Props = ObjectType.GetProperties();
            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo Prop = Props[i];
                object[] Attributes = Prop.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    if (Attribute.FieldType.Contains("id_"))
                    {
                        continue;
                    }

                    HtmlTag Label = new HtmlTag("p", Prop.Name.ToUpper());

                    HtmlTag InputField = new HtmlTag("input");
                    InputField.Class = "form-control";
                    InputField.ID = Prop.Name;
                    InputField.Name = "input-entity";
                    InputField.AddAttribute("type", "text");
                    Form.Add(Label);
                    Form.Add(InputField);
                }
            }

            //Action
            HtmlTag Action = new HtmlTag("input");
            Action.Name = "Action";
            Action.AddAttribute("type", "hidden");
            Action.AddAttribute("value", "Post");

            Form.Add(Action);

            HtmlTag Button = new HtmlTag("input");
            Button.Class = "btn btn-success";
            Button.AddAttribute("type", "submit");
            Button.AddAttribute("value", "Send");
            Form.Add(new BreakLine());
            Form.Add(Button);

            return new HtmlString(ControlUtil.HtmlTagToString(Form));
        }
    }
}
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
            string IDProp = ObjectUtil.GetIDProps(ObjectType);
            object ID = null;
            foreach (object Obj in Collection)
            {
                HtmlTag Tr = new HtmlTag("tr");
                Tr.ID = "row-obj-" + Idx;
                Tr.Add(new HtmlTag("td", Idx.ToString()));
                foreach (PropertyInfo Prop in CustomedProp)
                {
                    if (Prop.Name.Equals(IDProp))
                    {
                        ID = Prop.GetValue(Obj);
                    }
                    HtmlTag Td = new HtmlTag("td", ObjectType.GetProperty(Prop.Name).GetValue(Obj).ToString());
                    Tr.Add(Td);
                }
                Idx++;
                HtmlTag Options = new HtmlTag("td");
                Options.Class = "btn-group";

                HtmlTag BtnEdit = new HtmlTag("button", "Edit");
                BtnEdit.Class = "btn btn-warning";
                BtnEdit.AddAttribute("onclick", "editEntity(" + ID + ")");
                HtmlTag BtnDelete = new HtmlTag("button", "Delete");
                BtnDelete.Class = "btn btn-danger";
                BtnDelete.AddAttribute("onclick", "deleteEntity(" + ID + ")");
                Options.AddAll(BtnEdit, BtnDelete);

                Tr.Add(Options);
                TBody.Add(Tr);
            }

            Table.Add(TBody);
            //   string LableStr = $ "<label style=\"background-color:gray;color:yellow;font-size:24px\">{Content}</label>";
            return ControlUtil.HtmlTagToString(Table);
        }

        public static IHtmlString GenerateForm(Type ObjectType, object Entity = null)
        {
            return new HtmlString(GenerateFormString(ObjectType, Entity));
        }

        public static string GenerateFormString(Type ObjectType, object Entity = null)
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
                    object Value = null;

                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    HtmlTag InputField = new HtmlTag("input");
                    HtmlTag InputHelper = null;
                    if (Attribute.FieldType.Contains("id_"))
                    {
                        if (Entity == null)
                            continue;
                        else
                            InputField.AddAttribute("disabled", "disabled");
                    }

                    bool IsTextArea = Attribute.FieldType.Equals(AttributeConstant.TYPE_TEXTAREA);
                    bool IsDate = Attribute.FieldType.Equals(AttributeConstant.TYPE_DATE);
                    bool IsNumber = Attribute.FieldType.Equals(AttributeConstant.TYPE_NUMBER);
                    bool IsDropDown = Attribute.FieldType.Equals(AttributeConstant.TYPE_DROPDOWN);
                    // bool IsTextArea = Attribute.FieldType.Equals(AttributeConstant.TYPE_TEXTAREA);

                    HtmlTag Label = new HtmlTag("p", Prop.Name.ToUpper());
                    if (Entity != null)
                    {
                        Value = Prop.GetValue(Entity);
                    }

                    if (IsTextArea)
                    {
                        InputField.Key = "textarea";
                        InputField.Value = Value == null ? "" : Value.ToString();
                    }
                    else if (IsDropDown)
                    {
                        InputField.Key = "select";
                        InputField.AddAttribute("multiple", "multiple");

                        InputHelper = new HtmlTag("input");
                        InputHelper.AddAttribute("autocomplete", "off");
                        InputHelper.Class = "form-control";
                        InputHelper.ID = "helper-" + Prop.Name;
                        InputHelper.AddAttribute("onkeyup", "fillComboBox('" + Prop.Name.Trim() + "','"
                            + StringUtil.ToUpperCase(0, Attribute.ClassReference) + "','"
                            + Attribute.ClassAttributeConverter + "',this)");

                        InputField.AddAttribute("onchange", "setValue('" + InputHelper.ID + "',this.options[this.selectedIndex].innerText)");

                    }
                    else
                    {
                        string type = "text";
                        if (IsDate)
                        {
                            type = "date";
                            if (Value != null)
                                Value = StringUtil.DateAcceptableForHtmlInput((DateTime)Value);
                        }
                        InputField.AddAttribute("type", type);
                        InputField.AddAttribute("value", Value == null ? "" : Value.ToString());
                    }



                    InputField.Class = "form-control";
                    InputField.ID = Prop.Name;
                    InputField.Name = "input-entity";

                    Form.Add(Label);
                    if (InputHelper != null)
                    {
                        Form.Add(InputHelper);
                    }
                    Form.Add(InputField);
                }
            }

            //Action
            HtmlTag Action = new HtmlTag("input");
            Action.Name = "Action";
            Action.AddAttribute("type", "hidden");
            Action.AddAttribute("value", "Post");

            Form.Add(Action);

            HtmlTag BtnWrapper = new HtmlTag("div");
            BtnWrapper.Class = "btn-group";
            HtmlTag BtnSubmit = new HtmlTag("input");
            BtnSubmit.Class = "btn btn-success";
            BtnSubmit.AddAttribute("type", "submit");
            BtnSubmit.AddAttribute("value", "Submit");

            HtmlTag BtnReset = new HtmlTag("a", "reset");
            BtnReset.Class = "btn btn-default";
            BtnReset.AddAttribute("onclick", "generateForm(null)");

            BtnWrapper.AddAll(BtnSubmit, BtnReset);

            Form.Add(new BreakLine());
            Form.Add(BtnWrapper);

            return ControlUtil.HtmlTagToString(Form);
        }
    }
}
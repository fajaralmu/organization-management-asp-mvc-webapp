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
            return new HtmlString(GenerateDataTableString(ObjectType, Collection));
        }

        public static string GenerateDataTableString(Type ObjectType, List<object> Collection)
        {
            HtmlTag Table = new HtmlTag("table");
            Table.Class = ("table");
            Table.ID = "table-list-entity";

            HtmlTag ColumnNames = new HtmlTag("tr");
            ColumnNames.AddAttribute("valign", "top");
            HtmlTag No = new HtmlTag("th", "#");
            No.AddAttribute("scope", "col");
            ColumnNames.Add(No);

            List<PropertyInfo> CustomedProp = new List<PropertyInfo>();

            PropertyInfo[] Props = ObjectType.GetProperties();
            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo Prop = Props[i];
                object[] Attributes = Prop.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length == 0)
                    continue;

                FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                if (Attribute.SkipInTable)
                    continue;

                string FieldName = Prop.Name;
                string ObjName = ObjectType.Name;
                bool ClassRef = false;
                if (StringUtil.NotNullAndNotBlank(Attribute.ClassReference))
                {
                    ClassRef = true;
                    FieldName = Attribute.ClassAttributeConverter;
                    ObjName = Attribute.ClassReference;
                }

                //filter
                HtmlTag FilterBox = new InputTag();
                if (Attribute.FieldType.Equals(AttributeConstant.TYPE_DATE))
                {
                    HtmlTag FilterDay = new InputTag() { ID = string.Concat("filter-", FieldName, ".day"), Class = "form-control", Name = "filter-box" };
                    FilterDay.AddAttribute("onkeyup", "filterEntity(this)", "style", "width:30%", "placeholder", "Day");

                    HtmlTag FilterMonth = new InputTag() { ID = string.Concat("filter-", FieldName, ".month"), Class = "form-control", Name = "filter-box" };
                    FilterMonth.AddAttribute("onkeyup", "filterEntity(this)", "style", "width:30%", "placeholder", "Month");

                    HtmlTag FilterYear = new InputTag() { ID = string.Concat("filter-", FieldName, ".year"), Class = "form-control", Name = "filter-box" };
                    FilterYear.AddAttribute("onkeyup", "filterEntity(this)", "style", "width:40%", "placeholder", "Year");

                    FilterBox.Class = "input-group";
                    FilterBox.Key = "div";
                    FilterBox.AddAll(FilterDay, FilterMonth, FilterYear);
                }
                else
                {
                    FilterBox.Name = "filter-box";
                    FilterBox.ID = "filter-" + (ClassRef ? ObjName : FieldName);
                    FilterBox.AddAttribute("onkeyup", "filterEntity(this)", "class", "form-control", "placeholder", (ClassRef ? ObjName : FieldName));
                }

                //sorting
                HtmlTag BtnAsc = new HtmlTag("button", "&#8657;");
                string FullFieldName = ObjName + "." + FieldName;
                BtnAsc.ID = "asc-" + FullFieldName;
                BtnAsc.AddAttribute("name", "button-sort",
                    "class", "btn btn-xs", "onclick", "sortEntity('" + FullFieldName + "','asc')");
                HtmlTag BtnDesc = new HtmlTag("button", "&#8659;");
                BtnDesc.ID = "desc-" + FullFieldName;
                BtnDesc.AddAttribute("name", "button-sort",
                    "class", "btn btn-xs", "onclick", "sortEntity('" + FullFieldName + "','desc')");

                //    HtmlTag SortWrapper = Wrap("btn-group", BtnAsc, BtnDesc);
                HtmlTag FilterWrapper = Wrap(null, DivLabel(ClassRef ? ObjName.ToUpper() : FieldName.ToUpper()), FilterBox, BtnAsc, BtnDesc);
                HtmlTag Th = new HtmlTag("th", FilterWrapper);

                ColumnNames.Add(Th);
                CustomedProp.Add(Prop);

            }
            ColumnNames.Add(new HtmlTag("th", "OPTION"));
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
                    FieldAttribute Attribute = (FieldAttribute)Prop.GetCustomAttributes(typeof(FieldAttribute), true)[0];

                    if (Attribute.SkipInTable)
                        continue;

                    object Value = ObjectType.GetProperty(Prop.Name).GetValue(Obj);

                    if (Attribute.FieldType.Equals(AttributeConstant.TYPE_DROPDOWN) && Value != null)
                    {
                        if (StringUtil.NotNullAndNotBlank(Attribute.Values, Attribute.ItemNames))
                        {
                            Value = ObjectUtil.GetValueOfArray(Value, Attribute.Values, Attribute.ItemNames);
                        }
                        else if (StringUtil.NotNullAndNotBlank(Attribute.ClassReference, Attribute.ClassAttributeConverter))
                        {
                            string ClassAttrName = Attribute.ClassRefPropName == null ? Attribute.ClassReference : Attribute.ClassRefPropName;
                            object ClassVal = ObjectUtil.GetValueFromProp(ClassAttrName, Obj);
                            object ClassConverter = ObjectUtil.GetValueFromProp(Attribute.ClassAttributeConverter, ClassVal);
                            Value = ClassConverter;
                        }
                    }

                    HtmlTag Td = new HtmlTag("td", Value);
                    Tr.Add(Td);
                }
                Idx++;
                HtmlTag Options = new HtmlTag("td");
                Options.Class = "btn-group";

                HtmlTag BtnEdit = new HtmlTag("button", "<span class=\"glyphicon glyphicon-edit\"></span>");
                BtnEdit.AddAttribute("class", "btn btn-warning", "onclick", "editEntity(" + ID + ")");
                HtmlTag BtnDelete = new HtmlTag("button", "<span class=\"glyphicon glyphicon-trash\"></span>");
                BtnDelete.AddAttribute("class", "btn btn-danger", "onclick", "deleteEntity(" + ID + ")");

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

        public static HtmlTag GenerateTable(int Col, HtmlTag[] Elements)
        {
            HtmlTag Table = new HtmlTag("table");
            int ColIdx = 1;
            HtmlTag CurrentTr = new HtmlTag("tr");
            List<HtmlTag> Tds = new List<HtmlTag>();
            for (int i = 0; i < Elements.Length; i++)
            {
                HtmlTag Tag = Elements[i];

                HtmlTag Td = new HtmlTag("td", Tag);
                Tds.Add(Td);
                if (ColIdx == Col || i == Elements.Length - 1)
                {
                    ColIdx = 0;
                    CurrentTr.Add(Tds);
                    Table.Add(CurrentTr);
                    Tds.Clear();
                    CurrentTr = new HtmlTag("tr");
                }
                ColIdx++;
            }

            return (Table);
        }

        public static string GenerateFormString(Type ObjectType, object Entity = null)
        {
            string ObjName = StringUtil.ToUpperCase(0, ObjectType.Name);
            HtmlTag Form = new HtmlTag("form");
            Form.ID = "form-entity";
            Form.AddAttribute("class", "form", "onsubmit", "return submitEvent(event)");

            List<HtmlTag> InputFields = new List<HtmlTag>();

            PropertyInfo[] Props = ObjectType.GetProperties();
            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo Prop = Props[i];
                object[] Attributes = Prop.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    object Value = null;

                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    HtmlTag InputField = new InputTag();
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
                        if (StringUtil.NotNullAndNotBlank(Attribute.Values, Attribute.ItemNames) && Attribute.ItemNames.Length > 0)
                        {
                            for (int j = 0; j < Attribute.Values.Length; j++)
                            {
                                HtmlTag Option = new HtmlTag("option", Attribute.ItemNames[j]);
                                if ((Entity != null && Value.ToString().Equals(Attribute.Values[j].ToString())))
                                    Option.AddAttribute("selected", "");
                                Option.AddAttribute("value", Attribute.Values[j].ToString());
                                InputField.Add(Option);
                            }
                        }
                        else if (StringUtil.NotNullAndNotBlank(Attribute.ClassReference, Attribute.ClassAttributeConverter))
                        {
                            InputField.AddAttribute("multiple", "multiple");

                            InputHelper = new InputTag();
                            InputHelper.ID = "helper-" + Prop.Name;
                            InputHelper.AddAttribute("class", "form-control", "autocomplete", "off",
                                "onkeyup", "fillComboBox('" + Prop.Name.Trim() + "','"
                                + StringUtil.ToUpperCase(0, Attribute.ClassReference) + "','"
                                + Attribute.ClassAttributeConverter + "',this)");

                            InputField.AddAttribute("onchange", "setValue('" + InputHelper.ID + "',this.options[this.selectedIndex].innerText)");
                            if (Entity != null)
                            {
                                string ClassAttr = Attribute.ClassRefPropName != null ? Attribute.ClassRefPropName : Attribute.ClassReference;
                                object ClassVal = ObjectUtil.GetValueFromProp(ClassAttr, Entity);
                                if (ClassVal != null)
                                {
                                    string ClassAttrValue = ObjectUtil.GetValueFromProp(Attribute.ClassAttributeConverter, ClassVal).ToString();
                                    HtmlTag Option = new HtmlTag("option", ClassAttrValue);
                                    Option.AddAttribute("selected", "", "value", Value.ToString());
                                    InputHelper.AddAttribute("value", ClassAttrValue);
                                    InputField.Add(Option);
                                }
                            }
                        }
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
                        InputField.AddAttribute("type", type, "value", Value == null ? "" : Value.ToString());
                    }

                    if (Attribute.Required)
                    {
                        InputField.AddAttribute("required", "true");
                    }

                    InputField.Class = "form-control";
                    InputField.ID = Prop.Name;
                    InputField.Name = "input-entity";

                    InputFields.Add(Label);
                    if (InputHelper != null)
                    {
                        InputFields.Add(Wrap(null, InputHelper, InputField));
                    }
                    else
                    {
                        InputFields.Add(InputField);
                    }
                }
            }

            //Action
            HtmlTag Action = new InputTag();
            Action.Name = "Action";
            Action.AddAttribute("type", "hidden", "value", "Post");

            InputFields.Add(Action);

            HtmlTag BtnSubmit = new InputTag();
            BtnSubmit.Class = "btn btn-success";
            BtnSubmit.AddAttribute("type", "submit", "value", "Submit");

            HtmlTag BtnReset = new HtmlTag("a", "reset");
            BtnReset.Class = "btn btn-default";
            BtnReset.AddAttribute("onclick", "generateForm(null)");

            InputFields.Add(Wrap("btn-group", BtnSubmit, BtnReset));

            HtmlTag Table = GenerateTable(2, InputFields.ToArray());
            Form.Add(Table);
            return ControlUtil.HtmlTagToString(Form);
        }

        public static HtmlTag DivLabel(string Text)
        {
            return new HtmlTag("div", Text);
        }

        public static HtmlTag Wrap(string Class, params HtmlTag[] Tags)
        {
            HtmlTag Wrapper = new HtmlTag("div");
            if (Class != null)
                Wrapper.Class = Class;
            foreach (HtmlTag Tag in Tags)
            {
                Wrapper.Add(Tag);
            }
            return Wrapper;
        }
    }
}
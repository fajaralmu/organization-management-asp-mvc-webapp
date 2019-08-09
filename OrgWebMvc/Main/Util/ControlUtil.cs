using OrgWebMvc.Main.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace InstApp.Util.Common
{
    public class ControlUtil
    {
        public static Label GenerateLabel(string Text, Color foreColor)
        {
            Label GenLabel = new Label();
            GenLabel.ForeColor = foreColor;
            GenLabel.Text = Text;
            return GenLabel;
        }
        public static Label GenerateLabel(string Text)
        {
            return GenerateLabel(Text, Color.Black);
        }

        public static string MapToHtmlString(Dictionary<string, object> Map)
        {
            string HTML = "";
            string KeyTag = "div";
            string innerHTML = "";
            List<string> Attrs = new List<string>();
            foreach (string Key in Map.Keys)
            {
                object Value = Map[Key];
                if (Key.Equals("Key"))
                {
                    Console.WriteLine("KEY: " + Value);
                    KeyTag = Value.ToString();
                }
                else if (Key.Equals("Value"))
                {
                    if (Value.GetType().Equals(typeof(Dictionary<string, object>)))
                    {
                        Console.WriteLine("MAP");
                        Value = MapToHtmlString((Dictionary<string, object>)Value);
                        innerHTML += Value;
                    }
                    else if (Value.GetType().Equals(typeof(List<Dictionary<string, object>>)))
                    {
                        Console.WriteLine("List-MAP");
                        foreach (Dictionary<string, object> map in (List<Dictionary<string, object>>)Value)
                        {
                            innerHTML += MapToHtmlString(map);
                        }
                    }
                    else
                    {
                        innerHTML += Value;
                    }
                }
                else
                {
                    Attrs.Add(Value.ToString());
                }
            }
            HTML = GenerateHtmlTag(KeyTag, Attrs.ToArray(), innerHTML);
            return HTML;
        }

        public static string HtmlTagToString(HtmlTag TAG)
        {
            string HTML = "";
            string KeyTag = TAG.Key;
            string innerHTML = "";
            List<string> Attrs = new List<string>();

            object Value = TAG.Value;

            if (Value != null && Value.GetType().Equals(typeof(HtmlTag)) && TAG.ListValueCount() == 0)
            {
                Value = HtmlTagToString((HtmlTag)Value);
                innerHTML += Value;
            }
            else if (TAG.ListValueCount() > 0)
            {
                foreach (HtmlTag map in TAG.GetListValue())
                {
                    innerHTML += HtmlTagToString(map);
                }
            }
            else
            {
                innerHTML += Value;
            }

            if (!TAG.HasAttribute("name"))
                TAG.AddAttribute("name", TAG.Name);
            if (!TAG.HasAttribute("id"))
                TAG.AddAttribute("id", TAG.ID);
            if (!TAG.HasAttribute("class"))
                TAG.AddAttribute("class", TAG.Class);
            HTML = GenerateHtmlTag(KeyTag, TAG.Attributes.ToArray(), innerHTML);
            return HTML;
        }

        public static string GenerateHtmlTag(string Tag, string[] Attribute, string InnerHTML)
        {
            string Html = "<" + Tag;

            if (Attribute != null && Attribute.Length > 0)
            {
                for (int i = 0; i < Attribute.Length; i++)
                {
                    Html += " " + Attribute[i] + " ";
                }
                Html += ">";
            }
            else
            {
                Html += ">";
            }
            Html += InnerHTML;
            Html += "</" + Tag + ">";
            return Html;
        }

        public static Table GenerateTableFromMap(Dictionary<string, object> Map)
        {
            Table Table = new Table();
            Table.CssClass = "table table-condensed";
            foreach (var key in Map.Keys)
            {
                TableRow Row = new TableRow();
                TableCell KeyCell = new TableCell();
                TableCell ValueCell = new TableCell();

                KeyCell.Controls.Add(GenerateLabel(key));
                ValueCell.Controls.Add(GenerateLabel((string)Map[key]));
                Row.Controls.Add(KeyCell);
                Row.Controls.Add(ValueCell);
                Table.Controls.Add(Row);

            }
            return Table;
        }

        public static Table GenerateTableFromArray(List<object[]> TableValues, bool UseHeader = true)
        {
            if (TableValues == null || TableValues.Count == 0)
            {
                return null;
            }
            Table Table = new Table();
            Table.CssClass = "table table-condensed";

            for (int i = 0; i < TableValues[0].Length; i++)
            {
                TableRow Row = new TableRow();
                if (UseHeader && i == 0)
                {
                    Row = new TableHeaderRow();
                }

                foreach (object[] keys in TableValues)
                {
                    if (keys == null || keys[i] == null)
                    {
                        continue;
                    }
                    TableCell KeyCell = new TableCell();
                    if (UseHeader && i == 0)
                    {
                        KeyCell = new TableHeaderCell();
                    }
                    KeyCell.Controls.Add(GenerateLabel((string)keys[i]));
                    Row.Controls.Add(KeyCell);
                }
                Table.Controls.Add(Row);
            }
            return Table;
        }
    }
}
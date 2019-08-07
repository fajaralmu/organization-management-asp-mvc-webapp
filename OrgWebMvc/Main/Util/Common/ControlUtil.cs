using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace OurLibrary.Util.Common
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
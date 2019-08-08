using InstApp.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgWebMvc.Main.Util
{
    public class HtmlTag
    {
        private static int Occ;

        private List<HtmlTag> ValueList = new List<HtmlTag>();
        public HtmlTag(string Key)
        {
            this.Key = Key;
            Init();
        }
        public HtmlTag(string Key, object Value)
        {
            this.Key = Key;
            this.Value = Value;
            Init();
        }

        public void Init()
        {
            Occ++;
            Attributes = new List<string>();
            Class = Key;
            Name = "custom-component";
            ID = DateTime.Now.Millisecond.ToString()+Occ;
        }

        public HtmlTag()
        {
            Init();
        }

        public string Key { get; set; }
        public object Value { get; set; }
        public string ID { get; set; }
        public string Class { get; set; }
        public string Name { get; set; }

        public List<string> Attributes { get; set; }
        public void Add(HtmlTag Tag)
        {
            ValueList.Add(Tag);

        }
        public void Add(List<HtmlTag> Tags)
        {
            foreach(HtmlTag Tag in Tags)
                ValueList.Add(Tag);

        }

        public void AddAll(params HtmlTag[] Tag)
        {
            for (int i = 0; i < Tag.Length; i++)
            {
                ValueList.Add(Tag[i]);
            }

        }
        public void Clear()
        {
            ValueList.Clear();
        }
        public int ListValueCount()
        {
            return ValueList.Count;
        }
        public List<HtmlTag> GetListValue()
        {
            return ValueList;
        }
        public void AddAttribute(string Key, string Value)
        {
            Attributes.Add(Key + "=\"" + Value + "\"");
        }

    }
}
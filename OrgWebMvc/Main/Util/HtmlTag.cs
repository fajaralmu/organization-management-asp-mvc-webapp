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
        public HtmlTag(string Key, HtmlTag Value)
        {
            this.Key = Key;
            this.Value = Value;
            Init();
            ValueList.Add(Value);
        }

        public void Init()
        {
            Occ++;
            Attributes = new List<string>();
            Class = Key;
            Name = "custom-component";
            ID = DateTime.Now.Millisecond.ToString() + Occ;
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
            foreach (HtmlTag Tag in Tags)
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
            RemoveIfExist(Key);
            Attributes.Add(Key + "=\"" + Value + "\"");
        }
        public void AddAttribute(params string[] KeynVal)
        {
            if (KeynVal.Length % 2 != 0)
            {
                return;
            }
            string CurrentKey = "";
            string CurrentVal = "";
            for (int i = 0; i < KeynVal.Length; i++)
            {

                if ((i + 1) % 2 != 0 || (i + 1) == 1) //odd value
                {
                    CurrentKey = KeynVal[i];
                }
                else //even value
                {
                    CurrentVal = KeynVal[i];
                    RemoveIfExist(CurrentKey);
                    AddAttribute(CurrentKey, CurrentVal);
                }
            }
        }

        private void RemoveIfExist(string Key)
        {
            foreach (string Attr in Attributes)
                if (Attr.ToLower().StartsWith(Key.ToLower() + "="))
                {
                    Attributes.Remove(Attr);
                    break;
                }

        }

        public bool HasAttribute(string Key)
        {
            foreach (string Attr in Attributes)
                if (Attr.ToLower().StartsWith(Key.ToLower() + "="))
                    return true;
            return false;
        }
    }
}
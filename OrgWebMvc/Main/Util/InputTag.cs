using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgWebMvc.Main.Util
{
    public class InputTag : HtmlTag
    {

        public InputTag(string type = null, object value = null)
        {
            if (type != null)
            {
                AddAttribute("type", type);
            }
            else
            {
              //  AddAttribute("type", "text");
            }
            if (value != null)
            {
                AddAttribute("value", value.ToString());
            }
            Init();
        }

        private void Init()
        {
            Key = "input";
        }
    }
}
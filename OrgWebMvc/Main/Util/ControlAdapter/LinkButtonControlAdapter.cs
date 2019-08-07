using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.Adapters;

namespace OurLibrary.Util.ControlAdapter
{
    public class LinkButtonControlAdapter:WebControlAdapter
    {
        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
           base.RenderBeginTag(writer);
        }
    }
}
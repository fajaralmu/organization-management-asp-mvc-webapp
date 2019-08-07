using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgWebMvc.Main.API
{
    public class WebResponse
    {
        public WebResponse()
        {
            code = 1;
            count = 0;
            message = "failed";
        }
        public WebResponse(int code, string message, object data = null, int count = 0)
        {
            this.code = code;
            this.message = message;
            this.data = data;
            this.count = count;

        }

        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public int count { get; set; }
    }
}
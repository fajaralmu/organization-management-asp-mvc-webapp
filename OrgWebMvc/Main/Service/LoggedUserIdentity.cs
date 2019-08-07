using OrgWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgWebMvc.Main.Service
{
    public class LoggedUserIdentity
    {
        public static user User { get; set; }
        public static String username { get; set; }
        public static String name {get;set;}

        public static void Clear()
        {
            User = null;
            username = null;
            name = null;
        }
    }
}
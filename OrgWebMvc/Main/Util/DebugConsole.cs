using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgWebMvc.Main.Util
{
    public class DebugConsole
    {
        /**
            write debug
        */
        public static void Debug(object Obj, String value)
        {
            System.Diagnostics.Debug.WriteLine("_____" + Obj.GetType().Name + " : " + value);
        }
    }
}
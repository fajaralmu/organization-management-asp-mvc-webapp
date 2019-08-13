using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Reflection;
using OrgWebMvc.Main.TxtResources;

namespace OrgWebMvc.Main.Util
{
    public class HtmlConstant
    {
        public static string ReadRftFile()
        {
            
            string currentDir = Path.GetDirectoryName(typeof(MainDir).Assembly.Location);

            string RFT = File.ReadAllText(currentDir+"/Rtf.txt");
            return RFT;
        }
    }
}
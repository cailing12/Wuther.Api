using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wuther.Util.Helper
{
    public static class StringHelper
    {
        public static string RemoveBlogContentSpecialChar(string str)
        {
            Regex regex = new Regex(@"<(.|\n)+?>");
            str = regex.Replace(str, "");
            str = str.Replace(" ", "");
            return str;
        }
    }
}

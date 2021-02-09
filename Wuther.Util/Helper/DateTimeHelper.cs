using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuther.Util.Helper
{
    public static class DateTimeHelper
    {
        public static string GetTimestamp(DateTime d)
        {
            TimeSpan ts = d.ToUniversalTime() - new DateTime(1970, 1, 1);
            var milliseconds = ts.TotalMilliseconds.ToString().Substring(0, 8).Replace(".", "");
            return milliseconds;
        }
    }
}

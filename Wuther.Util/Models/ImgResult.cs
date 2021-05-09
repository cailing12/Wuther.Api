using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuther.Util.Models
{
    public class ImgResult
    {
        public int Errno { get; set; }

        public IEnumerable<ImgInfo> Data { get; set; }
    }

    public class ImgInfo
    {
        public string Url { get; set; }

        public string alt { get; set; }

        public string href { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Wuther.Util.Models
{
    public class UserAddWithWrittenOffTimeDto : UserAddDto
    {
        public DateTime WrittenOffTime { get; set; }
    }
}

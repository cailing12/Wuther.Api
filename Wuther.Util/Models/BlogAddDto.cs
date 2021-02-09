using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuther.Util.Models
{
    public class BlogAddDto
    {   
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int MenuId { get; set; }
    }
}

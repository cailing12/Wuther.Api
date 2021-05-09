using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuther.Util.Models
{
    public class BlogFullDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public int? Like { get; set; }
        public int? Comment { get; set; }
        public int? Trend { get; set; }
        public DateTime CreateTime { get; set; }
        public string Path { get; set; }
        public int IsPublic { get; set; }

        public virtual MenuDto Menu { get; set; }
        public virtual UserDto User { get; set; }
    }
}

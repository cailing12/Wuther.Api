using System;
using System.Collections.Generic;

namespace Wuther.Entities.Models
{
    public partial class Blogs
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
        public DateTime? ModifyTime { get; set; }
        public string Path { get; set; }
        public int IsPublic { get; set; }

        public virtual Menus Menu { get; set; }
        public virtual Users User { get; set; }
    }
}

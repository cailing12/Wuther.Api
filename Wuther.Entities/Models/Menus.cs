using System;
using System.Collections.Generic;

namespace Wuther.Entities.Models
{
    public partial class Menus
    {
        public Menus()
        {
            Blogs = new HashSet<Blogs>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Position { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public int? Seqno { get; set; }

        public virtual ICollection<Blogs> Blogs { get; set; }
    }
}

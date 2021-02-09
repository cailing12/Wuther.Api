using System;
using System.Collections.Generic;
using System.Text;
using Wuther.Util.Enums;

namespace Wuther.Util.Models
{
    public class MenuFullDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MenuPosition Position { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public int Seqno { get; set; }
    }
}

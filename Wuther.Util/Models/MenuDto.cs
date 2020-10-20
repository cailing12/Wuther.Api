using System;
using System.Collections.Generic;
using System.Text;
using Wuther.Util.Enums;

namespace Wuther.Util.Models
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PositionDisPlay { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
    }
}

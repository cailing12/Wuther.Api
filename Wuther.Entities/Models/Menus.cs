using System;
using System.Collections.Generic;

namespace Wuther.Entities.Models
{
    public partial class Menus
    {
        public int Id { get; set; }
        public string CascadeId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public sbyte IsLeaf { get; set; }
        public string IconName { get; set; }
        public int Status { get; set; }
        public string ParentName { get; set; }
        public int SortNo { get; set; }
        public int? ParentId { get; set; }
        public sbyte IsSys { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;

namespace Wuther.Entities.Models
{
    public partial class Menus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Position { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
    }
}

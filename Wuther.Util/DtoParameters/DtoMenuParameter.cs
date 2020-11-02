using System;
using System.Collections.Generic;
using System.Text;
using Wuther.Util.Enums;

namespace Wuther.Util.DtoParameters
{
    public class DtoMenuParameter
    {
        private const int MaxPageSize = 20;
        public string Name { get; set; }
        public MenuPosition Position { get; set; } = MenuPosition.horizontal;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string Fields { get; set; }

        public string OrderBy { get; set; } = "Name";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wuther.Util.DtoParameters
{
    public class DtoUserParameter
    {
        private const int MaxPageSize = 20;
        public string Username { get; set; }
        public string Account { get; set; }
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string Fields { get; set; }

        public string OrderBy { get; set; } = "Username";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuther.Util.DtoParameters
{
    public class DtoBlogParameter
    {
        private const int MaxPageSize = 20;
        public int MenuId{get;set;}

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Abstract { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string Fields { get; set; }

        public string OrderBy { get; set; } = "CreateTime";
    }
}

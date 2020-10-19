using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wuther.Util.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Username { get; set; }
        public string GenderDisplay { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public string Test { get; set; }
    }
}

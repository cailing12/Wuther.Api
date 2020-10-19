using System;
using System.Collections.Generic;
using System.Text;
using Wuther.Util.Enums;

namespace Wuther.Util.Models
{
    public class UserFullDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Gender GenderDisplay { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }

        public DateTime? WrittenOffTime { get; set; }
    }
}

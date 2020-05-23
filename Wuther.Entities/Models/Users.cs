﻿using System;
using System.Collections.Generic;

namespace Wuther.Entities.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? Sex { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
    }
}

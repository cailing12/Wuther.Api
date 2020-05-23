using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Wuther.Entities.Models;

namespace Wuther.Api.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class LoginController : ControllerBase {
        DbContext context;
        public LoginController (DbContext context) {
            this.context = context;
        }

        [HttpPost]
        public IActionResult Post (Users user) {
            var list = context.Set<Users> ().Where (c => c.Account == user.Username && c.Password == user.Password);
            if (list.Any ()) {
                return new JsonResult (new { code = 200, msg = "成功", token = user.Username + user.Password });
            } else {
                return new JsonResult (new { code = -1, msg = "失败" });
            }

        }

        [HttpGet]
        public IActionResult Get () {
            return new JsonResult ("AAAAAAAAA");
        }

        [HttpDelete]
        public IActionResult Delete () {
            return new JsonResult ("BBBBBB");
        }
    }
}
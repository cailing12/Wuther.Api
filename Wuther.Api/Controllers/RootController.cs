using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wuther.Util.DtoParameters;

namespace Wuther.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>()
            {
                new LinkDto(Url.Link(nameof(GetRoot),new { }),"self","Get"),

                new LinkDto(Url.Link(nameof(UsersController.GetUsers),new { }),"get_users","Get"),

                new LinkDto(Url.Link(nameof(UsersController.CreateUser),new { }),"create_users","Post"),
            };

            return Ok(links);
        }
    }
}

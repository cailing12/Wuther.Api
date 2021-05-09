using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wuther.Bussiness.Interface;
using Wuther.Entities.Models;
using Wuther.Util.Models;

namespace Wuther.Api.Controllers
{
    [Route("api/Login")]
    [Authorize]
    public class LoginController : BaseController
    {
        IUserRepository _userRepository;
        public LoginController(IUserRepository userRepository, ILogger<LoginController> logger)
        {
            logger.LogTrace("login struct success");
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginDto user)
        {
            var result = await _userRepository.Login(user.Account, user.Password);
            if(result != null)
            {
                var token = _userRepository.GetToken(result);
                return Ok(new { token,result });
            }
            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return new JsonResult("BBBBBB");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
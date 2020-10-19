using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wuther.Bussiness.Interface;
using Wuther.Entities.Models;
using Wuther.Util.Models;

namespace Wuther.Api.Controllers
{
    [Route("api/Login")]
    public class LoginController : BaseController
    {
        IUserRepository _userRepository;
        public LoginController(IUserRepository userRepository)
        {
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
    }
}
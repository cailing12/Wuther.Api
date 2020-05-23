using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wuther.Bussiness.Interface;
using Wuther.Entities.Models;

namespace Wuther.Api.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class UsersController : ControllerBase {
        private IUserRepository _repository;
        public UsersController (IUserRepository repository) {
            _repository = repository;
        }

        [HttpGet]
        public async Task<Users> Get () {
            return await _repository.GetDataListAsync (1);
        }
    }
}
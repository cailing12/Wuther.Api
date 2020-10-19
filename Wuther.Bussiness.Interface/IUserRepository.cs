using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wuther.Entities.Models;
using Wuther.Util.DtoParameters;
using Wuther.Util.Helper;

namespace Wuther.Bussiness.Interface
{
    public interface IUserRepository : IRepository<Users>
    {
        Task<Users> GetUserAsync(int id);
        Task<PagedList<Users>> GetUsersAsync(DtoUserParameter parameter);
        Task<Users> Login(string account, string password);
        Task<Users> LoginByToken(string account);
        string GetToken(Users result);
    }
}
using System.Linq;
using System.Threading.Tasks;
using Wuther.Entities.Models;

namespace Wuther.Bussiness.Interface {
    public interface IUserRepository : IRepository<Users> {
        Task<Users> GetDataListAsync (int id);
    }
}
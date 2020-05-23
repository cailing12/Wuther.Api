using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wuther.Bussiness.Interface;
using Wuther.Entities.Models;

namespace Wuther.Bussiness.Service {
    public class UserRepository : Repository<Users>, IUserRepository {
        private DbContext _context;
        public UserRepository (DbContext context) : base (context) {
            _context = context;
        }

    public async Task<Users> GetDataListAsync(int id)
    {
      return await _context.FindAsync<Users>(id);
    }
  }
}
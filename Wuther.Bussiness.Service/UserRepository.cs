using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wuther.Bussiness.Interface;
using Wuther.Entities.Models;
using Wuther.Util.DtoParameters;
using Wuther.Util.Helper;
using Wuther.Util.Models;
using Wuther.Util.PropertyMapping;

namespace Wuther.Bussiness.Service
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        private DbContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        public UserRepository(DbContext context, IPropertyMappingService propertyMappingService) : base(context)
        {
            _context = context;
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public string GetToken(Users result)
        {
            return result.Account + result.Password;
        }

        public async Task<Users> GetUserAsync(int id)
        {
            return await _context.FindAsync<Users>(id);
        }

        public async Task<PagedList<Users>> GetUsersAsync(DtoUserParameter parameter)
        {
            var queryExpression = _context.Set<Users>() as IQueryable<Users>;
            if (!string.IsNullOrWhiteSpace(parameter.Username))
            {
                queryExpression = queryExpression.Where(c => c.Username.Contains(parameter.Username));
            }
            if (!string.IsNullOrWhiteSpace(parameter.Account))
            {
                queryExpression = queryExpression.Where(c => c.Account == parameter.Account);
            }
            var mappingDictionary = _propertyMappingService.GetPropertyMapping<UserDto, Users>();
            queryExpression = queryExpression.ApplySort(parameter.OrderBy, mappingDictionary);

            return await PagedList<Users>.CreateAsync(queryExpression, parameter.PageNumber, parameter.PageSize);
        }

        public async Task<Users> Login(string account, string password)
        {
            var user = _context?.Set<Users>().Where(c => c.Account == account && c.Password == password);
            if (user.Any())
            {
                return await user.FirstAsync();
            }
            else
            {
                return null;
            }
        }

        public Task<Users> LoginByToken(string account)
        {
            throw new System.NotImplementedException();
        }
    }
}
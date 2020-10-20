using System.Threading.Tasks;
using Wuther.Entities.Models;
using Wuther.Util.DtoParameters;
using Wuther.Util.Helper;

namespace Wuther.Bussiness.Interface
{
    public interface IMenuRepository:IRepository<Menus>
    {
        Task<Menus> GetMenuAsync(int id);
        Task<PagedList<Menus>> GetMenusAsync(DtoMenuParameter parameter);
    }
}

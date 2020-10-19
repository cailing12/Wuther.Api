using System.Threading.Tasks;
using Wuther.Entities.Models;

namespace Wuther.Bussiness.Interface
{
    public interface IMenuRepository:IRepository<Menus>
    {
        Task<Menus> GetMenuAsync(int id);
    }
}

using System.Threading.Tasks;
using Wuther.Entities.Models;
using Wuther.Util.DtoParameters;
using Wuther.Util.Helper;

namespace Wuther.Bussiness.Interface
{
    public interface IBlogsRepository : IRepository<Blogs>
    {
        Task<Blogs> GetBlogAsync(int id);
        Task<PagedList<Blogs>> GetBlogsAsync(DtoBlogParameter parameter);
    }
}

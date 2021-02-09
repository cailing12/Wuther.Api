using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wuther.Bussiness.Interface;
using Wuther.Entities.Models;
using Wuther.Util.DtoParameters;
using Wuther.Util.Helper;
using Wuther.Util.Models;
using Wuther.Util.PropertyMapping;

namespace Wuther.Bussiness.Service
{
    public class BlogsRepository : Repository<Blogs>, IBlogsRepository
    {
        private readonly DbContext _context;
        private readonly IPropertyMappingService _propertyMappingService;
        public BlogsRepository(DbContext context, IPropertyMappingService propertyMappingService) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }
        public async Task<Blogs> GetBlogAsync(int id)
        {
            var blog =  _context.Find<Blogs>(id);
            await _context.Entry(blog).Reference(c => c.Menu)
           .LoadAsync();
            await _context.Entry(blog).Reference(c => c.User)
          .LoadAsync();
            //await EagerLoadAsync(blog, c => c.Menu);
            //await EagerLoadAsync<Users>(blog, d => d.User);
            return blog;
        }

        public async Task<PagedList<Blogs>> GetBlogsAsync(DtoBlogParameter parameter,bool eager)
        {
            IQueryable<Blogs> queryExpression;
            //是否预加载
            if (eager)
            {
                queryExpression = _context.Set<Blogs>().Include(c => c.Menu).Include(d => d.User);
            }
            else
            {
                queryExpression = _context.Set<Blogs>();
            }
            
            if (!string.IsNullOrWhiteSpace(parameter.Title))
            {
                queryExpression = queryExpression.Where(c => c.Title.Contains(parameter.Title));
            }
            if (!string.IsNullOrWhiteSpace(parameter.Abstract))
            {
                queryExpression = queryExpression.Where(c => c.Abstract.Contains(parameter.Abstract));
            }
            if (parameter.UserId != 0)
            {
                queryExpression = queryExpression.Where(c => c.UserId == parameter.UserId);
            }
            if (parameter.MenuId != 0)
            {
                queryExpression = queryExpression.Where(c => c.MenuId == parameter.MenuId);
            }
            if (parameter.StartTime != null && parameter.EndTime != null)
            {
                queryExpression = queryExpression.Where(c => c.CreateTime >= parameter.StartTime && c.CreateTime <= parameter.EndTime);
            }


            var mappingDictionary = _propertyMappingService.GetPropertyMapping<BlogDto, Blogs>();
            queryExpression = queryExpression.ApplySort(parameter.OrderBy, mappingDictionary);
            return await PagedList<Blogs>.CreateAsync(queryExpression, parameter.PageNumber, parameter.PageSize);
        }
    }
}

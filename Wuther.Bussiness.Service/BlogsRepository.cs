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
            return await _context.FindAsync<Blogs>(id);
        }

        public async Task<PagedList<Blogs>> GetBlogsAsync(DtoBlogParameter parameter)
        {
            var queryExpression = _context.Set<Blogs>() as IQueryable<Blogs>;
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

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
    public class MenuRepository : Repository<Menus>, IMenuRepository
    {
        private readonly DbContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        public MenuRepository(DbContext context, IPropertyMappingService propertyMappingService) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public async Task<Menus> GetMenuAsync(int id)
        {
            return await _context.FindAsync<Menus>(id);
        }

        public async Task<PagedList<Menus>> GetMenusAsync(DtoMenuParameter parameter)
        {
            var queryExpression = _context.Set<Menus>() as IQueryable<Menus>;
            if (!string.IsNullOrWhiteSpace(parameter.Name))
            {
                queryExpression = queryExpression.Where(c => c.Name.Contains(parameter.Name));
            }
            queryExpression = queryExpression.Where(c => c.Position == (int)parameter.Type);

            var mappingDictionary = _propertyMappingService.GetPropertyMapping<MenuDto, Menus>();
            queryExpression = queryExpression.ApplySort(parameter.OrderBy, mappingDictionary);

            return await PagedList<Menus>.CreateAsync(queryExpression, parameter.PageNumber, parameter.PageSize);
        }
    }
}

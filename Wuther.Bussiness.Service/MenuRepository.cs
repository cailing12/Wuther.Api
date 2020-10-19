using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wuther.Bussiness.Interface;
using Wuther.Entities.Models;
using Wuther.Util.PropertyMapping;

namespace Wuther.Bussiness.Service
{
    public class MenuRepository: Repository<Menus>, IMenuRepository
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
    }
}

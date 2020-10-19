using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Wuther.Util.PropertyMapping
{
    public class PropertyCheckerService : IPropertyCheckerService
    {
        public bool TypeHasProperties<TSource>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }
            var fieldsAfterSplit = fields.Split(",");
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propertyInfo == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

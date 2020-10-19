using System.Collections.Generic;

namespace Wuther.Util.PropertyMapping
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<TSource, TDestnation>(string fields);
    }
}
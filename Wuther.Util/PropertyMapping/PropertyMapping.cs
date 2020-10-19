using System;
using System.Collections.Generic;
using System.Text;

namespace Wuther.Util.PropertyMapping
{
    public class PropertyMapping<TSource,TDestination>:IPropertyMapping
    {
        public Dictionary<string,PropertyMappingValue> MappingDictionary { get; set; }

        public PropertyMapping(Dictionary<string,PropertyMappingValue> mappingDictionary)
        {
            MappingDictionary = mappingDictionary ?? throw new ArgumentNullException(nameof(mappingDictionary));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace Wuther.Util.PropertyMapping
{
    public static class ObjectExtension
    {
        public static ExpandoObject ShapeData<TSource>(this TSource source,string fields)
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var expandObject = new ExpandoObject();
            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                foreach(PropertyInfo propertyInfo in propertyInfos)
                {
                    var propertValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandObject).Add(propertyInfo.Name, propertValue);
                }
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");
                foreach(var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.Public|BindingFlags.Instance|BindingFlags.IgnoreCase);
                    if(propertyInfo == null)
                    {
                        throw new Exception($"在{typeof(TSource)}上没有找到{propertyName}");
                    }
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandObject).Add(propertyName, propertyValue);
                }
            }
            return expandObject;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Wuther.Entities.Models;
using Wuther.Util.Models;

namespace Wuther.Util.PropertyMapping
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _userPropertyMapping =
            new Dictionary<string, PropertyMappingValue>()
            {
                {"Id",new PropertyMappingValue(new List<string>{"Id" },true) },
                {"Account",new PropertyMappingValue(new List<string>{"Account" }) },
                {"Username",new PropertyMappingValue(new List<string>{"Username" }) },
                {"GenderDisplay",new PropertyMappingValue(new List<string>{"Sex" }) },
                {"Email",new PropertyMappingValue(new List<string>{"Email" }) },
                {"Department",new PropertyMappingValue(new List<string>{"Department" }) },
                {"Phone",new PropertyMappingValue(new List<string>{"Phone" }) },
                {"Test",new PropertyMappingValue(new List<string>{"Username" }) },
            };

        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<UserDto, Users>(_userPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            var propertyMappings = matchingMapping.ToList();
            if (propertyMappings.Count == 1)
            {
                return matchingMapping.First().MappingDictionary;
            }
            throw new Exception($"无法找到唯一的映射关系:{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestnation>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource,TDestnation>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }
            var fieldAfterSplit = fields.Split(",");
            foreach(var filed in fieldAfterSplit)
            {
                var trimmedFiled = filed.Trim();
                var indexOfFirstSpace = trimmedFiled.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1 ? trimmedFiled 
                    : trimmedFiled.Remove(indexOfFirstSpace);
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

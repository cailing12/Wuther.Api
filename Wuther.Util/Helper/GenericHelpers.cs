using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wuther.Util.Models;

namespace Wuther.Util.Helper
{
    public static class GenericHelpers
    {
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
           this IEnumerable<T> collection,
           Func<T, K> idSelector,
           Func<T, K> parentIdSelector,
           K rootId = default(K))
        {
            var list = collection.Where(u =>
            {
                var selector = parentIdSelector(u);
                return (rootId == null && selector == null)
                       || (rootId != null && rootId.Equals(selector));
            });
            foreach (var c in list)
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(idSelector, parentIdSelector, idSelector(c))
                };
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Wuther.Util.Models
{
    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
}

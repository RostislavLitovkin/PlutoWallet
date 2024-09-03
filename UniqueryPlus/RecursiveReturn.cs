using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniqueryPlus
{
    /// <summary>
    /// Think of a better name :)
    /// </summary>
    public class RecursiveReturn<T>
    {
        public byte[]? LastKey { get; set; }
        public required IEnumerable<T> Items { get; set; }
    }
}

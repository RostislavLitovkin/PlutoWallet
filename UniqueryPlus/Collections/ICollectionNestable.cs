
using System.Numerics;

namespace UniqueryPlus.Collections
{
    public interface ICollectionNestable
    {
        bool IsNestableByTokenOwner { get; set; }
        bool IsNestableByCollectionOwner { get; set; }
        IEnumerable<BigInteger>? RestrictedByCollectionIds { get; set; }
    }
}

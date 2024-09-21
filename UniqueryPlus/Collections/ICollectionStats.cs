using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UniqueryPlus.Collections
{
    public interface ICollectionStats
    {
        public BigInteger HighestSale { get; set; }
        public BigInteger FloorPrice { get; set; }
        public BigInteger Volume { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UniqueryPlus.Collections
{
    public class CollectionMintConfig : ICollectionMintConfig
    {
        public uint? NftMaxSuply { get; set; }
        public required MintType MintType { get; set; }
        public BigInteger? MintStartBlock { get; set; }
        public BigInteger? MintEndBlock { get; set; }
        public BigInteger? MintPrice { get; set; }
    }
}

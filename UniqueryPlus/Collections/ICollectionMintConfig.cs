using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UniqueryPlus.Collections
{
    /// <summary>
    /// https://paritytech.github.io/polkadot-sdk/master/src/pallet_nfts/types.rs.html#296
    /// </summary>
    public class MintType {
        public MintTypeEnum Type { get; set; }
        public uint? CollectionId { get; set; } = null;
    }
    public enum MintTypeEnum
    {
        None,
        Unknown,
        Public,
        Issuer,
        HolderOfCollection,
        Whitelist,
        CannotMint,
    }

    /// <summary>
    /// https://paritytech.github.io/polkadot-sdk/master/src/pallet_nfts/types.rs.html#367
    /// </summary>
    public interface ICollectionMintConfig
    {
        public uint? NftMaxSuply { get; set; }
        public MintType MintType { get; set; }
        public BigInteger? MintStartBlock { get; set; }
        public BigInteger? MintEndBlock { get; set; }
        public BigInteger? MintPrice { get; set; }
    }
}

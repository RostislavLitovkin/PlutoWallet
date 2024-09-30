using System;
using System.Numerics;
using PlutoWallet.Constants;

namespace PlutoWallet.Types
{
    public class AssetMetadata
    {
        public string Symbol { get; set; }
        public string ChainIcon { get; set; }
        public string DarkChainIcon { get; set; }
        public Endpoint Endpoint { get; set; }
        public AssetPallet Pallet { get; set; }
        public BigInteger AssetId { get; set; }
        public int Decimals { get; set; }

        public Asset ToAsset()
        {
            return new Asset
            {
                Symbol = Symbol,
                ChainIcon = ChainIcon,
                DarkChainIcon = DarkChainIcon,
                Endpoint = Endpoint,
                Pallet = Pallet,
                AssetId = AssetId,
                Decimals = Decimals,
                Amount = 0,
                UsdValue = 0
            };
        }
    }
	public class Asset : AssetMetadata
	{
        public double Amount { get; set; }
        public double UsdValue { get; set; }
    }

    public enum AssetPallet
    {
        Native,
        Assets,
        Tokens,
    }
}


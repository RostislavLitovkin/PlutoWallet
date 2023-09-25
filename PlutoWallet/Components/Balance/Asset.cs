using System;
using System.Numerics;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.Balance
{
	public class Asset
	{
        public double Amount { get; set; }
        public string Symbol { get; set; }
        public string ChainIcon { get; set; }
        public Endpoint Endpoint { get; set; }
        public double UsdValue { get; set; }
        public AssetPallet Pallet { get; set; }
        public BigInteger AssetId { get; set; }
    }

    public enum AssetPallet
    {
        Native,
        Assets,
        Tokens,
    }
}


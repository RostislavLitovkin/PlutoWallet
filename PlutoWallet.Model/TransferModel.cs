using System;
using System.Linq;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Newtonsoft.Json;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Substrate.NetApi.Generated.Model.sp_runtime.multiaddress;
using PlutoWallet.Types;
using System.Numerics;

namespace PlutoWallet.Model
{
	public class TransferModel
	{
		public static Method NativeTransfer(SubstrateClientExt client, string address, BigInteger amount)
		{
            // Later: Recognize what type of the address it is and convert it into ss58 one
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(0, accountId);

            var baseComAmount = new BaseCom<U128>();
            baseComAmount.Create(amount);

            var (palletIndex, callIndex) = PalletCallModel.GetPalletAndCallIndex(client, "Balances", "transfer_keep_alive");

            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(multiAddress.Encode());
            byteArray.AddRange(baseComAmount.Encode());
            return new Method(palletIndex, "Balances", callIndex, "transfer_keep_alive", byteArray.ToArray());
        }

        public static Method AssetsTransfer(SubstrateClientExt client, string address, BigInteger assetId, CompactInteger amount)
        {
            // Even if the assetId is different type than U128,
            // like for example U32, it will still result in the same bytes after the .Encode().
            var baseComAssetId = new BaseCom<U128>();
            baseComAssetId.Create(assetId);

            // Later: Recognize what type of the address it is and convert it into ss58 one
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(0, accountId);

            var baseComAmount = new BaseCom<U128>();
            baseComAmount.Create(amount);

            var (palletIndex, callIndex) = PalletCallModel.GetPalletAndCallIndex(client, "Assets", "transfer_keep_alive");

            Console.WriteLine("Pallet index: " + palletIndex + "    Call index: " + callIndex);

            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(baseComAssetId.Encode());
            byteArray.AddRange(multiAddress.Encode());
            byteArray.AddRange(baseComAmount.Encode());
            return new Method(palletIndex, "Assets", callIndex, "transfer_keep_alive", byteArray.ToArray());
        }
    }
}


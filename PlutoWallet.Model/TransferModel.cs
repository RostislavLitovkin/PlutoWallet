using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using PlutoWallet.Model.AjunaExt;
using Polkadot.NetApi.Generated.Model.sp_core.crypto;
using System.Numerics;
using Polkadot.NetApi.Generated.Model.sp_runtime.multiaddress;

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
            byteArray.AddRange(client.Endpoint.AddressVersion switch
            {
                0u => accountId.Encode(),
                // Maybe handle more variants?
                _ => multiAddress.Encode(),
            });
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

        public static Method TokensTransfer(SubstrateClientExt client, string address, BigInteger assetId, CompactInteger amount)
        {
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            // Later: Check that the chain really supports U32 for token ids
            U32 currencyId = new U32((uint)assetId);

            var baseComAmount = new BaseCom<U128>();
            baseComAmount.Create(amount);

            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(accountId.Encode());
            byteArray.AddRange(currencyId.Encode());
            byteArray.AddRange(baseComAmount.Encode());

            var (palletIndex, callIndex) = PalletCallModel.GetPalletAndCallIndex(client, "Tokens", "transfer_keep_alive");

            Console.WriteLine("Pallet index: " + palletIndex + "    Call index: " + callIndex);

            return new Method(palletIndex, "Tokens", callIndex, "transfer_keep_alive", byteArray.ToArray());
        }
    }
}


using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Types;
using PlutoWallet.Model.HydraDX;

namespace PlutoWallet.Model
{
    public class FeeModel
	{
        /**
         * Gets a transfer fee for the currently selected chain
         */
        public static async Task<Asset> GetNativeTransferFeeAsync(SubstrateClientExt client)
        {
            Method transfer = TransferModel.NativeTransfer(client, "5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ", 1000000000);

            return await GetMethodFeeAsync(client, transfer);
        }

        /**
         * Gets a transfer fee for the currently selected chain
         */
        public static async Task<Asset> GetAssetsTransferFeeAsync(SubstrateClientExt client)
        {
            Method transfer = TransferModel.AssetsTransfer(client, "5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ", 1, 1000000000);

            return await GetMethodFeeAsync(client, transfer);
        }

        /**
        * Gets a transfer fee for the currently selected chain
        */
        public static async Task<Asset> GetMethodFeeAsync(SubstrateClientExt client, Method method)
        {
            
            var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(method, MockModel.GetMockAccount(), 64, CancellationToken.None);

            var feeDetail = await client.SubstrateClient.Payment.QueryFeeDetailAsync(
                Utils.Bytes2HexString(extrinsic.Encode()),
                null,
                CancellationToken.None);

            var amount = (double)(feeDetail.InclusionFee.BaseFee.Value + feeDetail.InclusionFee.AdjustedWeightFee.Value + feeDetail.InclusionFee.LenFee.Value) / Math.Pow(10, client.Endpoint.Decimals); ;

            var usdValue = Sdk.GetSpotPrice(client.Endpoint.Unit);

            var asset = new Asset
            {
                Symbol = client.Endpoint.Unit,
                Decimals = client.Endpoint.Decimals,
                Amount = amount,
                Endpoint = client.Endpoint,
                Pallet = AssetPallet.Native,
                AssetId = 0,
                ChainIcon = client.Endpoint.Icon,
                DarkChainIcon = client.Endpoint.DarkIcon,
                UsdValue = usdValue * amount,
            };

            return asset;
        }

        public static async Task<RuntimeDispatchInfo> GetPaymentInfoAsync(SubstrateClientExt client, UnCheckedExtrinsic extrinsic)
        {
            Hash blockHash = await client.SubstrateClient.Chain.GetFinalizedHeadAsync(CancellationToken.None);

            return await client.SubstrateClient.Payment.QueryInfoAsync(
                Utils.Bytes2HexString(extrinsic.Encode()),
                Utils.Bytes2HexString(blockHash.Encode()),
                CancellationToken.None);
        }

        public static string GetEstimatedFeeString(Asset? asset)
        {
            if (asset == null)
            {
                return "Estimated fee: unknown";
            }

            var amount = Math.Abs(asset.Amount);
            var isLessThan = (amount < 0.01) ? "<" : "";
            return $"Estimated fee: {isLessThan}{String.Format("{0:0.00}", amount)} {asset.Symbol}";
        }
    }
}


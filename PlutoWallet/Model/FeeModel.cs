using System;
using System.Numerics;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi;
using Newtonsoft.Json;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Meta;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;
using PlutoWallet.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using Substrate.NetApi.Model.Types;
using Chaos.NaCl;
using Schnorrkel;
using PlutoWallet.Constants;
using PlutoWallet.Model;


namespace PlutoWallet.Model
{
	public class FeeModel
	{
        /**
         * Gets you a string version of transfer fee for the currently selected chain
         */
        public static async Task<string> GetNativeTransferFeeStringAsync()
        {
            Endpoint endpoint = Model.AjunaClientModel.SelectedEndpoint;
            BigInteger fee = await GetNativeTransferFeeAsync();
            return (double)fee / Math.Pow(10, endpoint.Decimals) + " " + endpoint.Unit;
        }

        /**
         * Gets a transfer fee for the currently selected chain
         */
        public static async Task<BigInteger> GetNativeTransferFeeAsync()
        {
            var client = Model.AjunaClientModel.Client;
            Method transfer = TransferModel.NativeTransfer(client, "5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ", 1000000000);

            return await GetMethodFeeAsync(transfer);
        }

        /**
         * Gets you a string version of transfer fee for the currently selected chain
         */
        public static async Task<string> GetAssetsTransferFeeStringAsync()
        {
            Endpoint endpoint = Model.AjunaClientModel.SelectedEndpoint;
            BigInteger fee = await GetAssetsTransferFeeAsync();
            return (double)fee / Math.Pow(10, endpoint.Decimals) + " " + endpoint.Unit;
        }

        /**
         * Gets a transfer fee for the currently selected chain
         */
        public static async Task<BigInteger> GetAssetsTransferFeeAsync()
        {
            var client = Model.AjunaClientModel.Client;
            Method transfer = TransferModel.AssetsTransfer(client, "5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ", 1, 1000000000);

            return await GetMethodFeeAsync(transfer);
        }

        /**
        * Gets a transfer fee for the currently selected chain
        */
        public static async Task<BigInteger> GetMethodFeeAsync(Method method)
        {
            var client = Model.AjunaClientModel.Client;

            UnCheckedExtrinsic extrinsic = await client.GetExtrinsicParametersAsync(
                method,
                MockModel.GetMockAccount(),
                client.DefaultCharge,
                lifeTime: 64,
                signed: true,
                CancellationToken.None);

            Console.WriteLine("Here is the extrinsics bytes: " + Utils.Bytes2HexString(extrinsic.Encode()));
            var feeDetail = await client.Payment.QueryFeeDetailAsync(
                Utils.Bytes2HexString(extrinsic.Encode()),
                null,
                CancellationToken.None);

            return feeDetail.InclusionFee.BaseFee.Value + feeDetail.InclusionFee.AdjustedWeightFee.Value + feeDetail.InclusionFee.LenFee.Value;
        }

        public static async Task<RuntimeDispatchInfoV1> GetPaymentInfoAsync(UnCheckedExtrinsic extrinsic)
        {
            var client = Model.AjunaClientModel.Client;

            Hash blockHash = await client.Chain.GetFinalizedHeadAsync(CancellationToken.None);

            return await client.Payment.QueryInfoAsync(
                Utils.Bytes2HexString(extrinsic.Encode()),
                Utils.Bytes2HexString(blockHash.Encode()),
                CancellationToken.None);
        }
    }
}


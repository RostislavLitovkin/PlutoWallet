﻿using System.Numerics;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Constants;


namespace PlutoWallet.Model
{
    public class FeeModel
	{
        /**
         * Gets you a string version of transfer fee for the currently selected chain
         */
        public static async Task<string> GetNativeTransferFeeStringAsync(SubstrateClientExt client, Endpoint endpoint)
        {
            BigInteger fee = await GetNativeTransferFeeAsync(client);
            return (double)fee / Math.Pow(10, endpoint.Decimals) + " " + endpoint.Unit;
        }

        /**
         * Gets a transfer fee for the currently selected chain
         */
        public static async Task<BigInteger> GetNativeTransferFeeAsync(SubstrateClientExt client)
        {
            Method transfer = TransferModel.NativeTransfer(client, "5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ", 1000000000);

            return await GetMethodFeeAsync(client, transfer);
        }

        /**
         * Gets you a string version of transfer fee for the currently selected chain
         */
        public static async Task<string> GetAssetsTransferFeeStringAsync(SubstrateClientExt client, Endpoint endpoint)
        {
            BigInteger fee = await GetAssetsTransferFeeAsync(client);
            return (double)fee / Math.Pow(10, endpoint.Decimals) + " " + endpoint.Unit;
        }

        /**
         * Gets a transfer fee for the currently selected chain
         */
        public static async Task<BigInteger> GetAssetsTransferFeeAsync(SubstrateClientExt client)
        {
            Method transfer = TransferModel.AssetsTransfer(client, "5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ", 1, 1000000000);

            return await GetMethodFeeAsync(client, transfer);
        }

        /**
        * Gets a transfer fee for the currently selected chain
        */
        public static async Task<BigInteger> GetMethodFeeAsync(SubstrateClientExt client, Method method)
        {
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

        public static async Task<RuntimeDispatchInfo> GetPaymentInfoAsync(SubstrateClientExt client, UnCheckedExtrinsic extrinsic)
        {
            Hash blockHash = await client.Chain.GetFinalizedHeadAsync(CancellationToken.None);

            return await client.Payment.QueryInfoAsync(
                Utils.Bytes2HexString(extrinsic.Encode()),
                Utils.Bytes2HexString(blockHash.Encode()),
                CancellationToken.None);
        }
    }
}


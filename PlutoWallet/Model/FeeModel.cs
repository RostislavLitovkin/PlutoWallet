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

namespace PlutoWallet.Model
{
	public class FeeModel
	{
        /**
         * Gets you a string version of transfer fee for the currently selected chain
         */
        public static async Task<string> GetTransferFeeStringAsync()
        {
            Endpoint endpoint = Model.AjunaClientModel.SelectedEndpoint;
            BigInteger fee = await GetTransferFeeAsync();
            return (double)fee / Math.Pow(10, endpoint.Decimals) + " " + endpoint.Unit;
        }

        /**
         * Gets a transfer fee for the currently selected chain
         */
        public static async Task<BigInteger> GetTransferFeeAsync()
        {
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom("5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ"));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Address32, accountId);

            var baseComAmount = new BaseCom<U128>();
            baseComAmount.Create(100);

            var client = Model.AjunaClientModel.Client;

            var (palletIndex, callIndex) = PalletCallModel.GetPalletAndCallIndex(client, "Balances", "transfer");

            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(multiAddress.Encode());
            byteArray.AddRange(baseComAmount.Encode());
            Method transfer = new Method(palletIndex, "Balances", callIndex, "transfer", byteArray.ToArray());

            var charge = ChargeTransactionPayment.Default();

            UnCheckedExtrinsic extrinsic = await client.GetExtrinsicParametersAsync(
                transfer,
                MockModel.GetMockAccount(),
                charge,
                lifeTime: 64,
                signed: true,
                CancellationToken.None);

            var feeDetail = await client.Payment.QueryFeeDetailAsync(
                Utils.Bytes2HexString(extrinsic.Encode()),
                null,
                CancellationToken.None);

            return feeDetail.InclusionFee.BaseFee.Value + feeDetail.InclusionFee.AdjustedWeightFee.Value + feeDetail.InclusionFee.LenFee.Value;
        }

        /**
        * Gets a transfer fee for the currently selected chain
        */
        public static async Task<BigInteger> GetMethodFeeAsync(Method method)
        {
            var client = Model.AjunaClientModel.Client;

            var charge = ChargeTransactionPayment.Default();

            UnCheckedExtrinsic extrinsic = await client.GetExtrinsicParametersAsync(
                method,
                MockModel.GetMockAccount(),
                charge,
                lifeTime: 64,
                signed: true,
                CancellationToken.None);

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


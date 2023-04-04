using System;
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

namespace PlutoWallet.Model
{
	public class FeeModel
	{
		public FeeModel()
		{
			
        }

        public static async Task<string> GetTransferFeeAsync()
        {
            // transfer
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom("5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ"));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(0, accountId);

            var baseComAmount = new BaseCom<U128>();
            baseComAmount.Create(100);

            var client = Model.AjunaClientModel.Client;

            var customMetadata = JsonConvert.DeserializeObject<Metadata>(client.MetaData.Serialize());

            var pallets = client.MetaData.NodeMetadata.Modules.Values.ToList<PalletModule>();

            int palletIndex = -1;

            for (int i = 0; i < pallets.Count; i++)
            {
                if (pallets[i].Name == "Balances")
                {
                    palletIndex = i;
                    break;
                }
            }

            if (palletIndex == -1)
            {
                throw new Exception("There is no Balances pallet.");
            }

            int callIndex = -1;

            for (int i = 0; i < customMetadata.NodeMetadata.Types[pallets[palletIndex].Calls.TypeId.ToString()].Variants.Count(); i++)
            {
                if (customMetadata.NodeMetadata.Types[pallets[palletIndex].Calls.TypeId.ToString()].Variants[i].Name == "transfer")
                {
                    callIndex = i;
                    break;
                }
            }

            if (palletIndex == -1)
            {
                throw new Exception("There is no transfer call.");
            }

            Console.WriteLine(palletIndex);
            Console.WriteLine(callIndex);

            Console.WriteLine("Pallet and Call Found ^^");

            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(multiAddress.Encode());
            byteArray.AddRange(baseComAmount.Encode());
            Method transfer = new Method((byte)palletIndex, "Balances", (byte)callIndex, "transfer", byteArray.ToArray());

            var charge = ChargeTransactionPayment.Default();

            UnCheckedExtrinsic extrinsic = await client.GetExtrinsicParametersAsync(
                transfer,
                KeysModel.GetAccount(),
                charge,
                lifeTime: 64,
                signed: true,
                CancellationToken.None);

            var account = KeysModel.GetAccount();
            var nonce = await client.System.AccountNextIndexAsync(account.Value, CancellationToken.None);

            Era era;
            Hash startEra;
            startEra = await client.Chain.GetFinalizedHeadAsync(CancellationToken.None);
            var finalizedHeader = await client.Chain.GetHeaderAsync(startEra, CancellationToken.None);
            era = Era.Create(64, finalizedHeader.Number.Value);

            var uncheckedExtrinsic =
                new UnCheckedExtrinsic(true, account, transfer, era, nonce, charge, client.GenesisHash, startEra);

            var payload = uncheckedExtrinsic.GetPayload(client.RuntimeVersion).Encode();

            /// Payloads longer than 256 bytes are going to be `blake2_256`-hashed.
            if (payload.Length > 256) payload = HashExtension.Blake2(payload, 256);

            byte[] signature;
            switch (account.KeyType)
            {
                case KeyType.Ed25519:
                    signature = Ed25519.Sign(payload, account.PrivateKey);
                    break;

                case KeyType.Sr25519:
                    signature = Sr25519v091.SignSimple(account.Bytes, account.PrivateKey, payload);
                    break;

                default:
                    throw new Exception($"Unknown key type found '{account.KeyType}'.");
            }

            uncheckedExtrinsic.AddPayloadSignature(signature);

            Console.WriteLine("TRANSFER FEE - I am here ^^ 1");
            Console.WriteLine("Signed: " + extrinsic.Signed);
            Console.WriteLine("Signature: " + Utils.Bytes2HexString(extrinsic.Signature));
            Console.WriteLine("2nd Signature: " + Utils.Bytes2HexString(signature));
            Console.WriteLine("Bytes: " + Utils.Bytes2HexString(extrinsic.Encode()));

            Hash blockHash = await client.Chain.GetFinalizedHeadAsync(CancellationToken.None);

            var feeDetail = await client.Payment.QueryFeeDetailAsync(
                Utils.Bytes2HexString(extrinsic.Encode()),
                "",
                CancellationToken.None);

            uint fee = (uint)(feeDetail.InclusionFee.BaseFee.Value + feeDetail.InclusionFee.AdjustedWeightFee.Value + feeDetail.InclusionFee.LenFee.Value);

            return "Fee: " + fee;
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


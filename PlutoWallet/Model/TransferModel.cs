using System;
using System.Linq;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Newtonsoft.Json;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;
using PlutoWallet.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using PlutoWallet.Types;

namespace PlutoWallet.Model
{
	public class TransferModel
	{

		public static async Task BalancesTransferAsync(string address, CompactInteger amount)
		{
            // Recognize what type of the address it is and convert it into ss58 one



            // transfer
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(0, accountId);

            var baseComAmount = new BaseCom<U128>();
            baseComAmount.Create(amount);

            var client = Model.AjunaClientModel.Client;

            var customMetadata = JsonConvert.DeserializeObject<Metadata>(client.MetaData.Serialize());

            var pallets = client.MetaData.NodeMetadata.Modules.Values.ToList<PalletModule>();

            int palletIndex = -1;

            for(int i = 0; i < pallets.Count; i++)
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

            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(multiAddress.Encode());
            byteArray.AddRange(baseComAmount.Encode());
            Method transfer = new Method((byte)palletIndex, "Balances", (byte)callIndex, "transfer", byteArray.ToArray());

            var charge = ChargeTransactionPayment.Default();

            var extrinsic = await client.GetExtrinsicParametersAsync(
                transfer,
                KeysModel.GetAccount(),
                charge,
                lifeTime: 64,
                signed: true,
                CancellationToken.None);

            

            await client.Author.SubmitExtrinsicAsync(Utils.Bytes2HexString(extrinsic.Encode()), CancellationToken.None);
        } 
	}
}


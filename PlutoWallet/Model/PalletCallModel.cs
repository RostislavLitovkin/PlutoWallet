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
    public class PalletNotFoundException : Exception
    {
        public PalletNotFoundException()
        {
        }

        public PalletNotFoundException(string message)
            : base(message)
        {
        }

        public PalletNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class CallNotFoundException : Exception
    {
        public CallNotFoundException()
        {
        }

        public CallNotFoundException(string message)
            : base(message)
        {
        }

        public CallNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class PalletCallModel
	{
        /**
         * Method that returns the pallet index and call index for the respective pallet and call names
         * 
         * First value: pallet index
         * Second value: call index
         */
		public static (byte, byte) GetPalletAndCallIndex(AjunaClientExt client, string palletName, string callName)
		{
            var customMetadata = JsonConvert.DeserializeObject<Metadata>(client.MetaData.Serialize());

            var pallets = client.MetaData.NodeMetadata.Modules.Values.ToList<PalletModule>();

            int palletIndex = -1;
            int metadataPalletIndex = -1;

            for (int i = 0; i < pallets.Count; i++)
            {
                //Console.WriteLine(i + ") " + pallets[i].Name + " :: " + pallets[i].Index);
                if (pallets[i].Name == palletName)
                {
                    palletIndex = (int)pallets[i].Index;
                    metadataPalletIndex = i;
                    break;
                }
            }

            if (metadataPalletIndex == -1)
            {
                throw new PalletNotFoundException("There is no Balances pallet.");
            }

            long callIndex = -1;

            for (int i = 0; i < customMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants.Count(); i++)
            {
                /*Console.WriteLine(
                    i + ") " +
                    customMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants[i].Name
                    + " :: " +
                    customMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants[i].Index);
                */

                //Console.WriteLine(customMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Path[0]);

                if (customMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants[i].Name == callName)
                {
                    callIndex = customMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants[i].Index;

                }
            }

            if (callIndex == -1)
            {
                throw new CallNotFoundException("There is no transfer call.");
            }

            return ((byte)palletIndex, (byte)callIndex);
        }
	}
}


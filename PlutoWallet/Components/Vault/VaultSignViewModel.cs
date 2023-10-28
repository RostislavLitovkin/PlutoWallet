using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Substrate.NetApi.Model.Extrinsics;
using Newtonsoft.Json;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi;
using PlutoWallet.Components.MessagePopup;

namespace PlutoWallet.Components.Vault
{
	public partial class VaultSignViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool isVisible;

        [ObservableProperty]
        private string palletIndex;

        [ObservableProperty]
        private string callIndex;

        [ObservableProperty]
        private string parameters;

        private Method ajunaMethod;

        public Method AjunaMethod
        {
            get => ajunaMethod;
            set
            {
                ajunaMethod = value;
                var client = Model.AjunaClientModel.Client;

                try
                {
                    

                    var pallet = client.MetaData.NodeMetadata.Modules[value.ModuleIndex];

                    PalletIndex = pallet.Name;
                    CallIndex = client.CustomMetadata.NodeMetadata.Types[pallet.Calls.TypeId.ToString()]
                        .Variants[value.CallIndex].Name;
                }
                catch
                {
                    PalletIndex = "(" + value.ModuleIndex.ToString() + " index)";
                    CallIndex = "(" + value.CallIndex.ToString() + " index)";
                }

                Parameters = Model.PalletCallModel.GetJsonMethod(client, value);
            }
        }

        public byte[] Payload { get; set; }

        [ObservableProperty]
        private string signature;

        [ObservableProperty]
        private bool signatureIsVisible = !false;

        [ObservableProperty]
        private bool signButtonIsVisible = true;

        public VaultSignViewModel()
		{
            signature = "Loading";
		}

		public void SignExtrinsic(string encodedBytes)
		{
            var client = Model.AjunaClientModel.Client;

            if (!client.IsConnected)
            {
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "Not connected";
                messagePopup.Text = "You need to connect to the chain. Check your Internet connection.";

                messagePopup.IsVisible = true;

                return;
            }

            SignatureIsVisible = !false;

            SignButtonIsVisible = true;

            Console.WriteLine(encodedBytes);

            Console.WriteLine("Scanned..");

            string address = Utils.GetAddressFrom(Utils.HexToByteArray(encodedBytes.Substring(0, 66)));

            Console.WriteLine("Address: " + address);

            Console.WriteLine("Method: " + encodedBytes.Substring(66));

            int offset = 0;

            int callLength = CompactInteger.Decode(Utils.HexToByteArray(encodedBytes.Substring(66, 8)), ref offset);

            Console.WriteLine(offset);
            Console.WriteLine(callLength);

            string methodBytes = encodedBytes.Substring(66 + offset * 2, callLength * 2);

            Method method = new Method(Utils.HexToByteArray(methodBytes.Substring(0, 2))[0], Utils.HexToByteArray(methodBytes.Substring(2, 2))[0], Utils.HexToByteArray(methodBytes.Substring(4)));

            AjunaMethod = method;
            Console.WriteLine("method: " + encodedBytes.Substring(66 + offset * 2, callLength * 2));

            string extensionBytes = encodedBytes.Substring(66 + offset * 2 + callLength * 2);

            Console.WriteLine("Orig: " + extensionBytes);

            string genesisHash = client.GenesisHash.Value;

            Console.WriteLine("Genesis hash: " + genesisHash);


            Console.WriteLine("These bytes are too long: " + extensionBytes.Length);
            extensionBytes = extensionBytes.Substring(0, extensionBytes.LastIndexOf(genesisHash.Substring(2)));


            Console.WriteLine("Not anymore: " + extensionBytes.Length);


            Console.WriteLine(extensionBytes);

            byte[] payload = Utils.HexToByteArray(methodBytes + extensionBytes);

            if (payload.Length > 256) payload = HashExtension.Blake2(payload, 256);

            Payload = payload;

            IsVisible = true;

            Console.WriteLine("Done");

            return;
            /*
            Console.WriteLine(payload.method);

            byte[] methodBytes = Utils.HexToByteArray(payload.method);

            List<byte> methodParameters = new List<byte>();

            for (int i = 2; i < methodBytes.Length; i++)
            {
                methodParameters.Add(methodBytes[i]);
            }

            Method method = new Method(methodBytes[0], methodBytes[1], methodParameters.ToArray());

            Hash eraHash = new Hash();
            eraHash.Create(Utils.HexToByteArray(payload.era));

            Hash blockHash = new Hash();
            blockHash.Create(payload.blockHash);

            Console.WriteLine("HexEra: " + payload.era);
            Console.WriteLine(eraHash);

            Hash genesisHash = new Hash();
            genesisHash.Create(Utils.HexToByteArray(payload.genesisHash));

            RuntimeVersion runtime = new RuntimeVersion
            {
                ImplVersion = payload.version,
                SpecVersion = HexStringToUint(payload.specVersion),
                TransactionVersion = HexStringToUint(payload.transactionVersion),
            };

            // This will need the AssetId update.
            // I was lazy now to fill it in, because it is very rare.
            ChargeType charge = payload.signedExtensions.Contains("ChargeAssetTxPayment") ?
                new ChargeAssetTxPayment(HexStringToUint(payload.tip), 0) :
                new ChargeTransactionPayment(HexStringToUint(payload.tip));

            var extrinsic = new Substrate.NetApi.Model.Extrinsics.UnCheckedExtrinsic(encodedBytes, ChargeTransactionPayment.Default());

            AjunaMethod = extrinsic.Method;
            Console.WriteLine("method..");

            Console.WriteLine(extrinsic.Account.Value);

            IsVisible = true;
            */
        }
	}
}


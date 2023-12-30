using System;
using Newtonsoft.Json;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.Constants;
using PlutoWallet.Types;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;

namespace PlutoWallet.Model
{
	public class PlutonicationModel
	{
		public static async Task ReceivePayload(Plutonication.Payload payload)
		{
            Console.WriteLine("Payload received");

            Console.WriteLine("genesis: " + payload.genesisHash);


            var transactionRequest = DependencyService.Get<TransactionRequestViewModel>();

            PlutoWalletSubstrateClient client;

            byte[] methodBytes = Utils.HexToByteArray(payload.method);

            List<byte> methodParameters = new List<byte>();

            for (int i = 2; i < methodBytes.Length; i++)
            {
                methodParameters.Add(methodBytes[i]);
            }

            Method method = new Method(methodBytes[0], methodBytes[1], methodParameters.ToArray());

            if (Endpoints.HashToKey.ContainsKey(payload.genesisHash))
            {
                var key = Endpoints.HashToKey[payload.genesisHash];

                Endpoint endpoint = Endpoints.GetEndpointDictionary[key];

                if (AjunaClientModel.GroupEndpointKeys.Contains(key))
                {
                    await AjunaClientModel.ChangeChainAsync(key);

                    client = AjunaClientModel.Client;
                }
                else
                {
                    string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

                    client = new PlutoWalletSubstrateClient(
                            endpoint,
                            new Uri(bestWebSecket),
                            Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

                    await client.ConnectAsync();
                }

                transactionRequest.EndpointKey = key;
                transactionRequest.ChainName = endpoint.Name;

                try
                {
                    var pallet = client.MetaData.NodeMetadata.Modules[method.ModuleIndex];
                    Metadata metadata = JsonConvert.DeserializeObject<Metadata>(client.MetaData.Serialize());

                    transactionRequest.PalletIndex = pallet.Name;
                    transactionRequest.CallIndex = metadata.NodeMetadata.Types[pallet.Calls.TypeId.ToString()]
                        .Variants[method.CallIndex].Name;



                    transactionRequest.CalculateFeeAsync(method);
                }
                catch
                {
                    transactionRequest.PalletIndex = "(" + method.ModuleIndex.ToString() + " index)";
                    transactionRequest.CallIndex = "(" + method.CallIndex.ToString() + " index)";

                    transactionRequest.Fee = "Fee: unknown";
                }

                
            }
            else
            {
                transactionRequest.ChainIcon = "";
                transactionRequest.ChainName = "Unknown";

                // Unknown
                transactionRequest.PalletIndex = "(" + method.ModuleIndex.ToString() + " index)";
                transactionRequest.CallIndex = "(" + method.CallIndex.ToString() + " index)";

                transactionRequest.Fee = "Fee: unknown";
            }

            transactionRequest.AjunaMethod = method;

            transactionRequest.Payload = payload;
            transactionRequest.IsVisible = true;

            if (method.Parameters.Length > 5)
            {
                transactionRequest.Parameters = "0x" + Convert.ToHexString(method.Parameters).Substring(0, 10) + "..";
            }
            else
            {
                transactionRequest.Parameters = "0x" + Convert.ToHexString(method.Parameters);
            }
        }
    }
}


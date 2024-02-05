using System;
using Newtonsoft.Json;
using Plutonication;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.Constants;
using PlutoWallet.Types;
using Substrate.NetApi;
using Substrate.NetApi.Generated.Model.sp_version;
using Substrate.NetApi.Model.Extrinsics;

namespace PlutoWallet.Model
{
	public class PlutonicationModel
	{
		public static async Task ReceivePayload(UnCheckedExtrinsic unCheckedExtrinsic, Substrate.NetApi.Model.Rpc.RuntimeVersion runtime)
		{
            var transactionRequest = DependencyService.Get<TransactionRequestViewModel>();

            PlutoWalletSubstrateClient client;

            Method method = unCheckedExtrinsic.Method;

            Substrate.NetApi.Model.Extrinsics.Payload payload = unCheckedExtrinsic.GetPayload(runtime);

            string genesisHash = Utils.Bytes2HexString(payload.SignedExtension.Genesis).ToLower();

            if (Endpoints.HashToKey.ContainsKey(genesisHash))
            {
                var key = Endpoints.HashToKey[genesisHash];

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

                    Task calculateFee = transactionRequest.CalculateFeeAsync(method);
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
                transactionRequest.Parameters = "0x" + Convert.ToHexString(method.ParametersBytes).Substring(0, 10) + "..";
            }
            else
            {
                transactionRequest.Parameters = "0x" + Convert.ToHexString(method.ParametersBytes);
            }
        }

        public static async Task ReceiveRaw(RawMessage message)
        {
            try
            {
                if (message.type != "bytes")
                {
                    throw new Exception("Message signing is supported only for bytes format");
                }

                var messageSignRequest = DependencyService.Get<MessageSignRequestViewModel>();

                messageSignRequest.Message = message;
                messageSignRequest.MessageString = message.data;
                messageSignRequest.IsVisible = true;
            }
            catch (Exception ex)
            {
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "ConnectionRequestView Error";
                messagePopup.Text = ex.Message;

                messagePopup.IsVisible = true;
            }
        }
    }
}


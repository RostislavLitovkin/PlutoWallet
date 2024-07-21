using Newtonsoft.Json;
using Plutonication;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.Constants;
using PlutoWallet.Types;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;

namespace PlutoWallet.Model
{
    public class PlutonicationModel
    {
        public static void ProcessAccessCredentials(AccessCredentials ac)
        {
            var connectionRequest = DependencyService.Get<ConnectionRequestViewModel>();

            connectionRequest.Show();
            connectionRequest.Icon = ac.Icon;
            connectionRequest.Name = ac.Name;

            connectionRequest.Url = ac.Url;
            connectionRequest.Key = ac.Key;
            connectionRequest.AccessCredentials = ac;

            DAppConnectionViewModel dAppViewModel = DependencyService.Get<DAppConnectionViewModel>();
            dAppViewModel.Icon = ac.Icon;
            dAppViewModel.Name = ac.Name;
            dAppViewModel.SetConnectionState(DAppConnectionStateEnum.Waiting);
            dAppViewModel.IsVisible = true;
        }        
        public static async Task AcceptConnectionAsync()
        {
            try
            {
                var viewModel = DependencyService.Get<ConnectionRequestViewModel>();

                viewModel.Connecting = true;

                viewModel.RequestViewIsVisible = false;
                viewModel.ConnectionStatusIsVisible = true;
                viewModel.ConnectionStatusText = $"Connecting.";

                DAppConnectionViewModel dAppViewModel = DependencyService.Get<DAppConnectionViewModel>();
                dAppViewModel.SetConnectionState(DAppConnectionStateEnum.Connecting);

                await PlutonicationWalletClient.InitializeAsync(
                    ac: viewModel.AccessCredentials,
                    pubkey: Model.KeysModel.GetSubstrateKey(),
                    signPayload: Model.PlutonicationModel.ReceivePayload,
                    signRaw: Model.PlutonicationModel.ReceiveRaw,
                    onConnected: (object sender, EventArgs args) =>
                    {
                        viewModel.Connecting = false;
                        viewModel.Connected = true;
                        viewModel.Confirming = true;

                        viewModel.ConnectionStatusText = $"Confirming.";

                        dAppViewModel.SetConnectionState(DAppConnectionStateEnum.Confirming);
                    },
                    onConfirmDAppConnection: () =>
                    {
                        dAppViewModel.SetConnectionState(DAppConnectionStateEnum.Connected);

                        viewModel.Confirming = false;
                        viewModel.Confirmed = true;
                        viewModel.ConnectionStatusText = $"Connected successfully. You can now go back to {viewModel.Name}.";
                    },
                    onDisconnected: (object sender, string args) =>
                    {
                        dAppViewModel.SetConnectionState(DAppConnectionStateEnum.Disconnected);
                    },
                    onReconnected: (object sender, int args) =>
                    {
                        dAppViewModel.SetConnectionState(DAppConnectionStateEnum.Reconnecting);
                    },
                    onReconnectFailed: (object sender, EventArgs args) =>
                    {
                        dAppViewModel.SetConnectionState(DAppConnectionStateEnum.Disconnected);
                    },
                    onDAppDisconnected: () =>
                    {
                        dAppViewModel.SetConnectionState(DAppConnectionStateEnum.Disconnected);
                    }
                );

                if (viewModel.PlutoLayout is not null)
                {
                    try
                    {
                        var plutoLayoutString = CustomLayoutModel.GetLayoutString(viewModel.PlutoLayout);
                        CustomLayoutModel.MergePlutoLayouts(plutoLayoutString);
                    }
                    catch
                    {
                        var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                        messagePopup.Title = "Outdated version";
                        messagePopup.Text = "Failed to import the dApp layout. Try updating PlutoWallet to newest version to fix this issue.";

                        messagePopup.IsVisible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "Connection Request Error";
                messagePopup.Text = ex.Message;

                messagePopup.IsVisible = true;
            }
        }
        public static async Task ReceivePayload(UnCheckedExtrinsic unCheckedExtrinsic, Substrate.NetApi.Model.Rpc.RuntimeVersion runtime)
        {
            var transactionRequest = DependencyService.Get<TransactionRequestViewModel>();

            PlutoWalletSubstrateClient client;

            Method method = unCheckedExtrinsic.Method;

            Substrate.NetApi.Model.Extrinsics.Payload payload = unCheckedExtrinsic.GetPayload(runtime);

            string genesisHash = Utils.Bytes2HexString(payload.SignedExtension.Genesis).ToLower();

            if (Endpoints.HashToKey.ContainsKey(genesisHash))
            {
                EndpointEnum key = Endpoints.HashToKey[genesisHash];

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

                    await client.ConnectAndLoadMetadataAsync();
                }

                transactionRequest.EndpointKey = key;
                transactionRequest.ChainName = endpoint.Name;

                try
                {
                    var pallet = client.SubstrateClient.MetaData.NodeMetadata.Modules[method.ModuleIndex];
                    Metadata metadata = JsonConvert.DeserializeObject<Metadata>(client.SubstrateClient.MetaData.Serialize());

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
                messageSignRequest.MessageString = GetMessageString(message.data);
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

        private static string GetMessageString(string hex)
        {
            int maxChars = 30;

            string result = hex.Length >= maxChars ? hex.Substring(0, maxChars) : hex;

            for (int i = 1; i <= hex.Length / maxChars; i++)
            {
                result += "\n" + (hex.Length - i * maxChars >= maxChars ? hex.Substring(i * maxChars, maxChars) : hex.Substring(i * maxChars));
            }

            return result;
        }
    }
}


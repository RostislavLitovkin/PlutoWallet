using Microsoft.Maui;
using Plutonication;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.Constants;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types;
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

            var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

            Method method = unCheckedExtrinsic.Method;

            Substrate.NetApi.Model.Extrinsics.Payload payload = unCheckedExtrinsic.GetPayload(runtime);

            string genesisHash = Utils.Bytes2HexString(payload.SignedExtension.Genesis).ToLower();

            if (Endpoints.HashToKey.ContainsKey(genesisHash))
            {
                EndpointEnum key = Endpoints.HashToKey[genesisHash];

                var client = await AjunaClientModel.GetOrAddSubstrateClientAsync(key);

                transactionRequest.EndpointKey = key;
                transactionRequest.ChainName = client.Endpoint.Name;

                try
                {
                    (var pallet, var call) = PalletCallModel.GetPalletAndCallName(client, method.ModuleIndex, method.CallIndex);

                    transactionAnalyzerConfirmationViewModel.PalletCallName = pallet + " " + call;

                    transactionAnalyzerConfirmationViewModel.Endpoint = client.Endpoint;

                    var dAppConnectionViewModel = DependencyService.Get<DAppConnectionViewModel>();

                    transactionAnalyzerConfirmationViewModel.DAppName = dAppConnectionViewModel.Name;
                    transactionAnalyzerConfirmationViewModel.DAppIcon = dAppConnectionViewModel.Icon;

                    transactionAnalyzerConfirmationViewModel.Payload = payload;

                    var account = new ChopsticksMockAccount();
                    account.Create(KeyType.Sr25519, KeysModel.GetPublicKeyBytes());

                    var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(method, account, 64, CancellationToken.None, signed: true);

                    Console.WriteLine(Utils.Bytes2HexString(extrinsic.Encode()).ToLower());

                    var events = await ChopsticksModel.SimulateCallAsync(client.Endpoint.URLs[0], extrinsic.Encode(), account.Value);

                    if (!(events is null))
                    {
                        var extrinsicDetails = await EventsModel.GetExtrinsicEventsForClientAsync(client, extrinsicIndex: events.ExtrinsicIndex, events.Events, blockNumber: 0, CancellationToken.None);

                        var currencyChanges = TransactionAnalyzerModel.AnalyzeEvents(extrinsicDetails.Events, client.Endpoint);

                        var analyzedOutcomeViewModel = DependencyService.Get<AnalyzedOutcomeViewModel>();

                        analyzedOutcomeViewModel.UpdateAssetChanges(currencyChanges);

                        transactionAnalyzerConfirmationViewModel.EstimatedFee = FeeModel.GetEstimatedFeeString(currencyChanges["fee"].First().Value);
                    }

                    transactionAnalyzerConfirmationViewModel.IsVisible = true;
                }
                catch
                {
                    transactionRequest.PalletIndex = "(" + method.ModuleIndex.ToString() + " index)";
                    transactionRequest.CallIndex = "(" + method.CallIndex.ToString() + " index)";

                    transactionRequest.Fee = "Fee: unknown";

                    transactionRequest.AjunaMethod = method;

                    transactionRequest.Payload = payload;
                    transactionRequest.IsVisible = true;
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

                transactionRequest.AjunaMethod = method;

                transactionRequest.Payload = payload;
                transactionRequest.IsVisible = true;
            }

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


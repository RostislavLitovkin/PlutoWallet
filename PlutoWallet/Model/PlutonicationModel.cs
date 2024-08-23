using Plutonication;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.Constants;
using PlutoWallet.Types;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types;
using AssetKey = (PlutoWallet.Constants.EndpointEnum, PlutoWallet.Types.AssetPallet, System.Numerics.BigInteger);
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

                    var header = await client.SubstrateClient.Chain.GetHeaderAsync(null, CancellationToken.None);


                    var xcmDestinationEndpointKey = XcmModel.IsMethodXcm(client.Endpoint, extrinsic.Method);

                    Console.WriteLine("XCM destination? " + xcmDestinationEndpointKey);

                    Dictionary<string, Dictionary<AssetKey, Asset>> currencyChanges = new Dictionary<string, Dictionary<AssetKey, Asset>>();
                    if (xcmDestinationEndpointKey is null)
                    {
                        var events = await ChopsticksModel.SimulateCallAsync(client.Endpoint.URLs[0], extrinsic.Encode(), header.Number.Value, account.Value);

                        if (!(events is null))
                        {
                            var extrinsicDetails = await EventsModel.GetExtrinsicEventsForClientAsync(client, extrinsicIndex: events.ExtrinsicIndex, events.Events, blockNumber: 0, CancellationToken.None);

                            currencyChanges = await TransactionAnalyzerModel.AnalyzeEventsAsync(client, extrinsicDetails.Events, client.Endpoint, CancellationToken.None);
                        }
                    }
                    else
                    {
                        var xcmResult = await ChopsticksModel.SimulateXcmCallAsync(
                            client.Endpoint.URLs[0],
                            Endpoints.GetEndpointDictionary[xcmDestinationEndpointKey ?? EndpointEnum.None].URLs[0],
                            extrinsic.Encode()
                        );

                        Console.WriteLine("XCM result received :)");


                        var destionationClient = await AjunaClientModel.GetOrAddSubstrateClientAsync(xcmDestinationEndpointKey ?? EndpointEnum.None);

                        if (!(xcmResult is null))
                        {
                            var fromExtrinsicDetails = await EventsModel.GetExtrinsicEventsForClientAsync(client, extrinsicIndex: xcmResult.FromEvents.ExtrinsicIndex, xcmResult.FromEvents.Events, blockNumber: 0, CancellationToken.None);

                            var toExtrinsicDetails = await EventsModel.GetExtrinsicEventsForClientAsync(destionationClient, extrinsicIndex: null, xcmResult.ToEvents.Events, blockNumber: 0, CancellationToken.None);

                            var fromCurrencyChanges = await TransactionAnalyzerModel.AnalyzeEventsAsync(client, fromExtrinsicDetails.Events, client.Endpoint, CancellationToken.None);

                            currencyChanges = await TransactionAnalyzerModel.AnalyzeEventsAsync(destionationClient, toExtrinsicDetails.Events, destionationClient.Endpoint, CancellationToken.None, existingCurrencyChanges: fromCurrencyChanges);
                        }
                    };

                    var analyzedOutcomeViewModel = DependencyService.Get<AnalyzedOutcomeViewModel>();

                    analyzedOutcomeViewModel.UpdateAssetChanges(currencyChanges);

                    transactionAnalyzerConfirmationViewModel.EstimatedFee = FeeModel.GetEstimatedFeeString(currencyChanges["fee"].First().Value);

                    transactionAnalyzerConfirmationViewModel.IsVisible = true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);

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


using Plutonication;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.Constants;
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
            Substrate.NetApi.Model.Extrinsics.Payload payload = unCheckedExtrinsic.GetPayload(runtime);

            string genesisHash = Utils.Bytes2HexString(payload.SignedExtension.Genesis).ToLower();

            var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

            if (Endpoints.HashToKey.ContainsKey(genesisHash))
            {
                EndpointEnum key = Endpoints.HashToKey[genesisHash];

                var client = await AjunaClientModel.GetOrAddSubstrateClientAsync(key);

                var signedExtensions = payload.SignedExtension;

                var tempPayload = new TempPayload(
                    payload.Call,
                    new TempSignedExtensions(
                        specVersion: signedExtensions.SpecVersion,
                        txVersion: signedExtensions.TxVersion,
                        genesis: signedExtensions.Genesis,
                        startEra: signedExtensions.StartEra,
                        mortality: signedExtensions.Mortality,
                        nonce: signedExtensions.Nonce,
                        charge: signedExtensions.Charge,
                        checkMetadata: client.CheckMetadata
                    )
                );

                await transactionAnalyzerConfirmationViewModel.LoadAsync(client, unCheckedExtrinsic.ToTempUnCheckedExtrinsic(payload, client.Endpoint.AddressVersion, client.CheckMetadata), true, onConfirm: OnConfirmClickedAsync, runtime);
            }
            else
            {
                transactionAnalyzerConfirmationViewModel.LoadUnknown(unCheckedExtrinsic.ToTempUnCheckedExtrinsic(payload, 2u, true), runtime, onConfirm: OnConfirmClickedAsync);
            }
        }

        public static async Task OnConfirmClickedAsync()
        {
            try
            {
                var viewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

                if ((await Model.KeysModel.GetAccount()).IsSome(out var account))
                {
                    Console.WriteLine("Authenticated");

                    byte[] signature = account.Sign(viewModel.Payload.Encode());

                    var signerResult = new SignerResult
                    {
                        id = 1,
                        signature = Utils.Bytes2HexString(
                            // This 1 means the signature is using Sr25519
                            new byte[1] { 1 }
                            .Concat(signature).ToArray()
                        ).ToLower(),
                    };

                    Console.WriteLine("Signed");

                    await PlutonicationWalletClient.SendPayloadSignatureAsync(signerResult);

                    Console.WriteLine("Sending");

                }

                // Hide this layout
                viewModel.IsVisible = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                // Handle potential errors
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


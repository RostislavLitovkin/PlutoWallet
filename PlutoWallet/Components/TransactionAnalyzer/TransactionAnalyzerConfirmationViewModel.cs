using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using AssetKey = (PlutoWallet.Constants.EndpointEnum, PlutoWallet.Types.AssetPallet, System.Numerics.BigInteger);

namespace PlutoWallet.Components.TransactionAnalyzer
{
    public partial class TransactionAnalyzerConfirmationViewModel : ObservableObject, IPopup, ISetToDefault
    {
        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private string dAppName;

        [ObservableProperty]
        private string dAppIcon;

        [ObservableProperty]
        private bool isDAppViewVisible;

        [ObservableProperty]
        private Endpoint endpoint;

        [ObservableProperty]
        private string palletCallName;

        [ObservableProperty]
        private TempPayload payload;

        [ObservableProperty]
        private string estimatedFee = "Estimated fee: Loading";

        // Estimated time should be calculated based the client
        [ObservableProperty]
        private string estimatedTime = "Estimated time: 6 sec";

        [ObservableProperty]
        private Func<Task> onConfirm;

        public async Task LoadAsync(SubstrateClientExt client, Method method, bool showDAppView, Func<Task> onConfirm, CancellationToken token = default)
        {
            var account = new ChopsticksMockAccount();
            account.Create(KeyType.Sr25519, KeysModel.GetPublicKeyBytes());

            #region Temp
            var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(method, account, lifeTime: 64, token: token);
            #endregion

            await LoadAsync(client, extrinsic, showDAppView, onConfirm);
        }
        public async Task LoadAsync(SubstrateClientExt client, TempUnCheckedExtrinsic unCheckedExtrinsic, bool showDAppView, Func<Task> onConfirm, RuntimeVersion? runtimeVersion = null)
        {
            OnConfirm = onConfirm;
            var analyzedOutcomeViewModel = DependencyService.Get<AnalyzedOutcomeViewModel>();

            Method method = unCheckedExtrinsic.Method;

            #region Basic Info
            Endpoint = client.Endpoint;
            Payload = unCheckedExtrinsic.GetPayload(runtimeVersion ?? client.SubstrateClient.RuntimeVersion);

            var dAppConnectionViewModel = DependencyService.Get<DAppConnectionViewModel>();

            

            if (showDAppView) {
                DAppName = dAppConnectionViewModel.Name;
                DAppIcon = dAppConnectionViewModel.Icon;
                IsDAppViewVisible = dAppConnectionViewModel.IsVisible;
            }
            else
            {
                /// Show just the endpoint
            }

            try
            {
                (var pallet, var call) = PalletCallModel.GetPalletAndCallName(client, method.ModuleIndex, method.CallIndex);

                PalletCallName = pallet + " " + call;
            }
            catch (Exception ex)
            {
                PalletCallName = "Unknown call";
            }
            #endregion

            IsVisible = true;

            #region other awaitable things
            try
            { 
                var account = new ChopsticksMockAccount();
                account.Create(KeyType.Sr25519, KeysModel.GetPublicKeyBytes());

                unCheckedExtrinsic.AddPayloadSignature(await account.SignAsync(Payload.Encode()));

                var header = await client.SubstrateClient.Chain.GetHeaderAsync(null, CancellationToken.None);

                var xcmDestinationEndpointKey = XcmModel.IsMethodXcm(client.Endpoint, unCheckedExtrinsic.Method);

                Dictionary<string, Dictionary<AssetKey, Asset>> currencyChanges = new Dictionary<string, Dictionary<AssetKey, Asset>>();
                if (xcmDestinationEndpointKey is null)
                {
                    var events = await ChopsticksModel.SimulateCallAsync(client.Endpoint.URLs[0], unCheckedExtrinsic.Encode(), header.Number.Value, account.Value);

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
                        unCheckedExtrinsic.Encode()
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

                analyzedOutcomeViewModel.UpdateAssetChanges(currencyChanges);

                analyzedOutcomeViewModel.Loading = "";

                EstimatedFee = FeeModel.GetEstimatedFeeString(currencyChanges.ContainsKey("fee") ? currencyChanges["fee"].First().Value : null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                analyzedOutcomeViewModel.Loading = "Failed";

                EstimatedFee = "Estimated fee: Unknown";
            }
            #endregion
        }

        public void LoadUnknown(TempUnCheckedExtrinsic unCheckedExtrinsic, RuntimeVersion runtimeVersion, Func<Task> onConfirm)
        {
            OnConfirm = onConfirm;

            var analyzedOutcomeViewModel = DependencyService.Get<AnalyzedOutcomeViewModel>();

            Method method = unCheckedExtrinsic.Method;

            #region Basic Info
            Endpoint = new Endpoint
            {
                Name = "Unknown",
                Key = EndpointEnum.None,
                URLs = new string[1] { "ws://127.0.0.1:8002" },
                Icon = "substrate.png",
                DarkIcon = "substrate.png",
                Unit = "",
                Decimals = 0,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
                ParachainId = new ParachainId
                {
                    Relay = RelayChain.Other,
                    Chain = Chain.Solo,
                    Id = null,
                }
            };

            Payload = unCheckedExtrinsic.GetPayload(runtimeVersion);


            var dAppConnectionViewModel = DependencyService.Get<DAppConnectionViewModel>();

            IsDAppViewVisible = dAppConnectionViewModel.IsVisible;

            if (dAppConnectionViewModel.IsVisible)
            {
                DAppName = dAppConnectionViewModel.Name;
                DAppIcon = dAppConnectionViewModel.Icon;
            }
            else
            {
                /// Show just the endpoint
            }

            PalletCallName = "Unknown call";

            analyzedOutcomeViewModel.Loading = "Unknown";

            EstimatedFee = "Estimated fee: Unknown";

            #endregion

            IsVisible = true;
        }

        public void SetToDefault()
        {
            IsVisible = false;
            DAppName = "";
            DAppIcon = "";
            IsDAppViewVisible = false;
            PalletCallName = "";
            Payload = null;
            EstimatedFee = "Estimated fee: Loading";
            EstimatedTime = "Estimated time: 6 sec";
            OnConfirm = null;

            var analyzedOutcomeViewModel = DependencyService.Get<AnalyzedOutcomeViewModel>();
            analyzedOutcomeViewModel.SetToDefault();
        }
    }
}

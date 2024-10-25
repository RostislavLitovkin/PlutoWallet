using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Extrinsics;

namespace PlutoWallet.Components.Nft
{
    internal partial class NftTransferViewModel : ObservableObject, IPopup, ISetToDefault
    {
        [ObservableProperty]
        private ButtonStateEnum confirmButtonState;

        private string address;

        public string Address { get => address; set { SetProperty(ref address, value); CheckCorrectInput(); } }

        [ObservableProperty]
        private string addressInput;

        [ObservableProperty]
        private string fee;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private Func<string, Method> transferFunction;

        [ObservableProperty]
        private EndpointEnum endpointKey;

        [ObservableProperty]
        private string errorMessage;
        public NftTransferViewModel()
        {
            SetToDefault();
        }

        [RelayCommand]
        public async Task TransferAsync()
        {
            ErrorMessage = "";

            var clientExt = await Model.AjunaClientModel.GetOrAddSubstrateClientAsync(EndpointKey);

            var client = clientExt.SubstrateClient;

            try
            {
                Method transfer = TransferFunction.Invoke(Address);

                /// Hide this layout
                SetToDefault();

                var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

                await transactionAnalyzerConfirmationViewModel.LoadAsync(clientExt, transfer, false);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Console.WriteLine(ex);
            }
        }

        public void CheckCorrectInput()
        {
            if (Address.Length == 48)
            {
                ConfirmButtonState = ButtonStateEnum.Enabled;
            }
            else
            {
                ConfirmButtonState = ButtonStateEnum.Disabled;
            }
        }

        public async Task GetFeeAsync()
        {
            if (EndpointKey is EndpointEnum.None || TransferFunction is null)
            {
                Fee = "Estimated fee: Failed";

                return;
            }

            Fee = "Estimated fee: Loading";

            var client = await Model.AjunaClientModel.GetOrAddSubstrateClientAsync(EndpointKey);
            if (client is null || !await client.IsConnectedAsync())
            {
                Fee = "Estimated fee: Failed";

                return;
            }

            try
            {
                var transfer = TransferFunction.Invoke("5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ");
                var feeAsset = await FeeModel.GetMethodFeeAsync(client, transfer);
                Fee = FeeModel.GetEstimatedFeeString(feeAsset);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nft estimated fee error: ");

                Console.WriteLine(ex);
                Fee = "Estimated fee: Unsupported";
            }
        }

        public void SetToDefault()
        {
            Address = "";
            AddressInput = "";
            IsVisible = false;
            Fee = "Estimated fee: Loading";
            ConfirmButtonState = ButtonStateEnum.Disabled;
        }
    }
}

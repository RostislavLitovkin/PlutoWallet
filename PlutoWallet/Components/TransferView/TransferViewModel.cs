using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.AssetSelect;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.HydraDX;
using PlutoWallet.Types;

namespace PlutoWallet.Components.TransferView
{
    public partial class TransferViewModel : ObservableObject, IPopup, ISetToDefault
    {
        [ObservableProperty]
        private ButtonStateEnum confirmButtonState;

        private string address;

        public string Address { get => address; set { SetProperty(ref address, value); CheckCorrectInput(); } }

        [ObservableProperty]
        private string addressInput;

        private string amount;

        public string Amount { get => amount; set { Console.WriteLine("Setting amount: " + value); SetProperty(ref amount, value); CheckCorrectInput(); } }

        [ObservableProperty]
        private string fee;

        [ObservableProperty]
        private bool isVisible;

        public TransferViewModel()
        {
            SetToDefault();
        }

        public void CheckCorrectInput()
        {
            if (Address.Length == 48 && decimal.TryParse(Amount, out decimal decimalAmount) && decimalAmount > 0)
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
            Fee = "Estimated fee: Loading";

            var mainClient = await Model.AjunaClientModel.GetMainClientAsync();
            if (mainClient is null || !await mainClient.IsConnectedAsync())
            {
                Fee = "Estimated fee: Failed";

                return;
            }

            try
            {
                var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

                var feeAsset = assetSelectButtonViewModel.SelectedAssetKey switch
                {
                    (EndpointEnum endpointKey, AssetPallet.Native, _) => await FeeModel.GetNativeTransferFeeAsync(mainClient),
                    (EndpointEnum endpointKey, AssetPallet.Assets, _) => await FeeModel.GetAssetsTransferFeeAsync(mainClient),
                    _ => throw new Exception("Not implemented")
                };

                Fee = FeeModel.GetEstimatedFeeString(feeAsset);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Estimated fee error: ");

                Console.WriteLine(ex);
                Fee = "Estimated fee: Unsupported";
            }
        }

        public void SetToDefault()
        {
            Address = "";
            AddressInput = "";
            Amount = "";
            IsVisible = false;
            Fee = "Estimated fee: Loading";
            ConfirmButtonState = ButtonStateEnum.Disabled;

            var assetInputViewModel = DependencyService.Get<AssetInputViewModel>();

            assetInputViewModel.Amount = "";
            assetInputViewModel.UsdAmount = "";
        }
    }
}


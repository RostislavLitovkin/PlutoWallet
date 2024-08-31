using System;
using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.AssetSelect;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Model;
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

        public string Amount { get => amount; set { SetProperty(ref amount, value); CheckCorrectInput(); } }

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
            Fee = "Estimated fee: loading";

            var mainClient = await Model.AjunaClientModel.GetMainClientAsync();
            if (mainClient is null || !await mainClient.IsConnectedAsync())
            {
                Fee = "Estimated fee: failed";

                return;
            }

            try
            {
                var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

                if (assetSelectButtonViewModel.SelectedAssetKey.Item2 == AssetPallet.Native)
                {
                    Fee = "Estimated fee: " + await FeeModel.GetNativeTransferFeeStringAsync(mainClient);
                }
                else if (assetSelectButtonViewModel.SelectedAssetKey.Item2 == AssetPallet.Assets)
                {
                    Fee = "Estimated fee: " + await FeeModel.GetAssetsTransferFeeStringAsync(mainClient);
                }
                else
                {
                    Fee = "Estimated fee: unsupported";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Estimated fee error: ");

                Console.WriteLine(ex);
                Fee = ex.Message;
            }
        }

        public void SetToDefault()
        {
            Address = "";
            AddressInput = "";
            Amount = "";
            IsVisible = false;
            Fee = "Estimated fee: loading";
            ConfirmButtonState = ButtonStateEnum.Disabled;
        }
    }
}


using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.AssetSelect;
using PlutoWallet.Model;
using PlutoWallet.Types;

namespace PlutoWallet.Components.TransferView
{
	public partial class TransferViewModel : ObservableObject, IPopup
    {
		[ObservableProperty]
		private string address;

		[ObservableProperty]
		private string amount;

        [ObservableProperty]
        private string fee;

        [ObservableProperty]
		private bool isVisible;

		public TransferViewModel()
		{
			SetToDefault();
			fee = "Fee: loading";
		}

		public async Task GetFeeAsync()
		{
            Fee = "Fee: loading";

            var mainClient = await Model.AjunaClientModel.GetMainClientAsync();
            if (mainClient is null || !await mainClient.IsConnectedAsync())
			{
				Fee = "Fee: Failed.";

				return;
			}

            try
			{
                var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

				if (assetSelectButtonViewModel.Pallet == AssetPallet.Native)
				{
					Fee = "Fee: " + await FeeModel.GetNativeTransferFeeStringAsync(mainClient);
				}
				else if (assetSelectButtonViewModel.Pallet == AssetPallet.Assets)
				{
					Fee = "Fee: " + await FeeModel.GetAssetsTransferFeeStringAsync(mainClient);
                }
                else
				{
					Fee = "Fee: Unsupported";
				}
			}
			catch (Exception ex)
			{
                Console.WriteLine("Fee error: ");

                Console.WriteLine(ex);
				Fee = ex.Message;
			}
        }

        public void SetToDefault()
        {
			Address = "";
			Amount = "";
			IsVisible = false;
        }
    }
}


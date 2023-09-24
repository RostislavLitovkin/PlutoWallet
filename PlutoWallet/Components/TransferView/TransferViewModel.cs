using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.AssetSelect;

namespace PlutoWallet.Components.TransferView
{
	public partial class TransferViewModel : ObservableObject
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
            try
			{
                var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

				if (assetSelectButtonViewModel.Pallet == Balance.AssetPallet.Native)
				{
					Fee = "Fee: " + await Model.FeeModel.GetNativeTransferFeeStringAsync();
				}
				else if (assetSelectButtonViewModel.Pallet == Balance.AssetPallet.Assets)
				{
					Fee = "Fee: " + await Model.FeeModel.GetAssetsTransferFeeStringAsync();
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


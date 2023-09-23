using System;
using CommunityToolkit.Mvvm.ComponentModel;

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
				Fee = "Fee: " + await Model.FeeModel.GetTransferFeeStringAsync();
			}
			catch (Exception ex)
			{
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


using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.TransferView
{
	public partial class TransferViewModel : ObservableObject
	{
		[ObservableProperty]
		private string address;

		[ObservableProperty]
		private int amount;

        [ObservableProperty]
        private string fee;

        [ObservableProperty]
		private bool isVisible;

		public TransferViewModel()
		{
			SetToDefault();
		}

		public async Task GetFeeAsync()
		{
			try
			{
				Fee = await Model.FeeModel.GetTransferFeeAsync();
			}
			catch (Exception ex)
			{
				Fee = ex.Message;
			}
        }

        public void SetToDefault()
        {
			Address = "";
			Amount = 0;
			IsVisible = false;
            Fee = "Fee: loading";
        }
    }
}


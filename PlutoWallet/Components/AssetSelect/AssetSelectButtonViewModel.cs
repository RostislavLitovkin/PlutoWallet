using System;
using PlutoWallet.Components.Balance;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Numerics;
using PlutoWallet.Constants;
using PlutoWallet.Types;

namespace PlutoWallet.Components.AssetSelect
{
	public partial class AssetSelectButtonViewModel : ObservableObject
	{
		[ObservableProperty]
		private ImageSource chainIcon;

		private string symbol;
        public string Symbol
		{
			get => symbol;
			set
			{
				WidthRequest = value.Length * 15 + 40;

                SetProperty(ref symbol, value);
			}
		}

		[ObservableProperty]
		private int widthRequest;

		public AssetPallet Pallet { get; set; }

		public BigInteger AssetId { get; set; }

		public Endpoint Endpoint { get; set; }

		public int Decimals { get; set; }

        public AssetSelectButtonViewModel()
		{
		}
	}
}


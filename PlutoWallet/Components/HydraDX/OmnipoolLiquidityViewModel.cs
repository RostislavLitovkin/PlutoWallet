using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.HydraDX
{
	public partial class OmnipoolLiquidityViewModel : ObservableObject
	{
		[ObservableProperty]
		private string liquidityAmount;

        [ObservableProperty]
        private string symbol;

        [ObservableProperty]
        private string usdValue;

        public OmnipoolLiquidityViewModel()
		{
			liquidityAmount = "Loading";
		}

		public async Task GetLiquidityAmount()
		{
			var omnipoolLiquidity = await Model.HydraDX.OmnipoolModel.GetOmnipoolLiquidityAmount();
			LiquidityAmount = String.Format("{0:0.0000}", omnipoolLiquidity.Amount);

			Symbol = omnipoolLiquidity.Symbol;

			double usdRatio = 0;

			UsdValue = "(" + String.Format("{0:0.00}", usdRatio * omnipoolLiquidity.Amount) + " USD)";
        }
    }
}


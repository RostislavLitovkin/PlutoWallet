using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Components.HydraDX
{
	public partial class OmnipoolLiquidityViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<AssetLiquidityInfo> assets;

		[ObservableProperty]
		private string usdSum;

        public OmnipoolLiquidityViewModel()
		{
			usdSum = "Loading";
		}

		public async Task GetLiquidityAmount(SubstrateClientExt client)
		{
			if (client is null || client.IsConnected)
			{
				UsdSum = "Failed";

                return;
			}

            UsdSum = "Loading";

            var omnipoolLiquidities = await Model.HydraDX.OmnipoolModel.GetOmnipoolLiquidityAmount(client, KeysModel.GetSubstrateKey());

			if (omnipoolLiquidities.Count() == 0)
			{
				UsdSum = "None";
				return;
			}

            double tempUsdSum = 0;

			Assets = new ObservableCollection<AssetLiquidityInfo>();

			foreach (var omnipoolLiquidity in omnipoolLiquidities) {
				double usdRatio = 0;

				double usdValue = usdRatio * omnipoolLiquidity.Amount;

				tempUsdSum += usdValue;

                Assets.Add(new AssetLiquidityInfo {
					Amount = String.Format("{0:0.00}", omnipoolLiquidity.Amount),
					Symbol = omnipoolLiquidity.Symbol,
					UsdValue = String.Format("{0:0.00}", usdValue) + " USD",
				});
			} 

			UsdSum = String.Format("{0:0.00}", tempUsdSum) + " USD";
        }
    }

	public class AssetLiquidityInfo
	{
		public string Amount { get; set; }
		public string Symbol { get; set; }
		public string UsdValue { get; set; }
	}
}


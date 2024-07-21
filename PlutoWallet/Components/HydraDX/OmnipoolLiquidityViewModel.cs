using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Hydration.NetApi.Generated;
using PlutoWallet.Model;

namespace PlutoWallet.Components.HydraDX
{
	public partial class OmnipoolLiquidityViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<AssetLiquidityInfo> assets;

        [ObservableProperty]
        private ObservableCollection<AssetLiquidityInfo> assetsLiquidityMining;

        [ObservableProperty]
		private string usdSum;

        public OmnipoolLiquidityViewModel()
		{
			usdSum = "Loading";
		}

		public async Task GetLiquidityAmount(SubstrateClientExt client)
		{
			if (client is null || !client.IsConnected)
			{
				UsdSum = "Failed";

                return;
			}

            UsdSum = "Loading";

            double tempUsdSum = 0;

			Assets = new ObservableCollection<AssetLiquidityInfo>();

			foreach (var omnipoolLiquidity in await Model.HydraDX.OmnipoolModel.GetOmnipoolLiquidityAmount(client, KeysModel.GetSubstrateKey())) {
                double usdRatio = Model.HydraDX.Sdk.GetSpotPrice(omnipoolLiquidity.AssetId);

                double usdValue = usdRatio * omnipoolLiquidity.Amount;

				tempUsdSum += usdValue;

                Assets.Add(new AssetLiquidityInfo {
					Amount = String.Format("{0:0.00}", omnipoolLiquidity.Amount),
					Symbol = omnipoolLiquidity.Symbol,
					UsdValue = String.Format("{0:0.00}", usdValue) + " USD",
                    LiquidityMiningInfos = []
                });
			}

            foreach (var omnipoolLiquidity in await Model.HydrationModel.HydrationLiquidityMiningModel.GetOmnipoolLiquidityWithLiquidityMining(client, KeysModel.GetSubstrateKey()))
            {
                double usdRatio = Model.HydraDX.Sdk.GetSpotPrice(omnipoolLiquidity.AssetId);

                double usdValue = usdRatio * omnipoolLiquidity.Amount;

                tempUsdSum += usdValue;

                Assets.Add(new AssetLiquidityInfo
                {
                    Amount = String.Format("{0:0.00}", omnipoolLiquidity.Amount),
                    Symbol = omnipoolLiquidity.Symbol,
                    UsdValue = String.Format("{0:0.00}", usdValue) + " USD",
                    LiquidityMiningInfos = omnipoolLiquidity.LiquidityMiningInfos.Select(lm => new LiquidityMiningInfo
                    {
                        Amount = String.Format("{0:0.00}", lm.RewardAmount),
                        Symbol = lm.RewardSymbol,
                        UsdValue = String.Format("{0:0.00}", lm.RewardAmount * Model.HydraDX.Sdk.GetSpotPrice(lm.RewardAssetId)) + " USD",
                    }).ToList(),
                });
            }

            UsdSum = String.Format("{0:0.00}", tempUsdSum) + " USD";


            if (Assets.Count() == 0)
            {
                UsdSum = "None";
                return;
            }
        }
    }

	public class AssetLiquidityInfo
	{
		public string Amount { get; set; }
		public string Symbol { get; set; }
		public string UsdValue { get; set; }
        public List<LiquidityMiningInfo> LiquidityMiningInfos { get; set; }
    }

    public class LiquidityMiningInfo
    {
        public string Amount { get; set; }
        public string Symbol { get; set; }
        public string UsdValue { get; set; }
    }
}


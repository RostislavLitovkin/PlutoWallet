using System;
using PlutoWallet.Model;
using PlutoWallet.Components.Balance;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using PlutoWallet.Types;

namespace PlutoWallet.Components.AssetSelect
{
	public class AssetSelect : Asset
	{
		public bool IsSelected { get; set; }
	}

	public partial class AssetSelectViewModel : ObservableObject, IPopup
    {
		[ObservableProperty]
		private bool isVisible;

		[ObservableProperty]
		private ObservableCollection<AssetSelect> assets = new ObservableCollection<AssetSelect>();

        public AssetSelectViewModel()
		{
			isVisible = false;
		}

		public async Task Appear()
		{
			IsVisible = true;

			var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

            List<AssetSelect> tempAssets = new List<AssetSelect>();

			foreach(Asset a in Model.AssetsModel.Assets)
			{
				bool isSelected = (
					assetSelectButtonViewModel.AssetId == a.AssetId &&
					assetSelectButtonViewModel.ChainIcon == a.ChainIcon &&
					assetSelectButtonViewModel.Pallet == a.Pallet &&
					assetSelectButtonViewModel.Symbol == a.Symbol &&
                    assetSelectButtonViewModel.Decimals == a.Decimals
                );

                tempAssets.Add(new AssetSelect
				{
					Amount = a.Amount,
					UsdValue = a.UsdValue,
					ChainIcon = a.ChainIcon,
					Pallet = a.Pallet,
					AssetId = a.AssetId,
					Endpoint = a.Endpoint,
					Symbol = a.Symbol,
					IsSelected = isSelected,
					Decimals = a.Decimals,
                });
			}

            Assets = new ObservableCollection<AssetSelect>(tempAssets);
        }
	}
}


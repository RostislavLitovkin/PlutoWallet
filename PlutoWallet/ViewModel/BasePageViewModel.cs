using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Components.Nft;
using PlutoWallet.View;

namespace PlutoWallet.ViewModel
{
	public partial class BasePageViewModel : ObservableObject
	{
        public MainView MainView = new MainView();

        private CancellationTokenSource nftsCancellationTokenSource;

        [ObservableProperty]
		private ContentView content;

		public BasePageViewModel()
		{
			content = MainView;
		}

        public void SetMainView()
		{
            var usdBalanceViewModel = DependencyService.Get<UsdBalanceViewModel>();
            usdBalanceViewModel.DoNotReload = true;

            // Handle the NFT view
            if (nftsCancellationTokenSource != null)
            {
                nftsCancellationTokenSource.Cancel(false);
                DependencyService.Get<NftLoadingViewModel>().IsVisible = false;
            }
            
            Content = MainView;

            var networkViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();
            networkViewModel.SetupDefault();
        }

        public async Task SetNftView()
        {
            Content = new NftView();

            if (nftsCancellationTokenSource != null)
            {
                nftsCancellationTokenSource.Dispose();
                nftsCancellationTokenSource = null;
            }

            nftsCancellationTokenSource = new CancellationTokenSource();
         
            try
            {
                await DependencyService.Get<NftViewModel>().GetNFTsAsync(nftsCancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                nftsCancellationTokenSource.Dispose();
                nftsCancellationTokenSource = null;
            }
        }
    }
}


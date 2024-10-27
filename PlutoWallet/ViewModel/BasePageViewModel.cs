using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.NavigationBar;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Components.Nft;
using PlutoWallet.Model;
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

            Console.WriteLine("Calling BasePageViewModel constructor.");

            var networkViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();
            networkViewModel.SetupDefault();
        }

        public void SetMainView()
		{
            var navigationBarViewModel = DependencyService.Get<NavigationBarViewModel>();

            navigationBarViewModel.NftsFontAttributes = FontAttributes.None;
            navigationBarViewModel.HomeFontAttributes = FontAttributes.Bold;

            Console.WriteLine("Changed to home");

            var usdBalanceViewModel = DependencyService.Get<UsdBalanceViewModel>();
            usdBalanceViewModel.DoNotReload = true;

            // Handle the NFT view
            if (nftsCancellationTokenSource != null)
            {
                nftsCancellationTokenSource.Cancel(false);
                DependencyService.Get<NftLoadingViewModel>().IsVisible = false;
            }
            
            Content = MainView;

            Console.WriteLine("SetMainView() -> Calling MultiNetworkSelectViewModel.SetupDefault()");

            var networkViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();
            networkViewModel.SetupDefault();
        }

        public async Task SetNftView()
        {
            Console.WriteLine("trying to change to nfts");

            var navigationBarViewModel = DependencyService.Get<NavigationBarViewModel>();

            navigationBarViewModel.NftsFontAttributes = FontAttributes.Bold;
            navigationBarViewModel.HomeFontAttributes = FontAttributes.None;

            Console.WriteLine("Changed to nfts");

            Content = new NftView();

            if (nftsCancellationTokenSource != null)
            {
                nftsCancellationTokenSource.Dispose();
                nftsCancellationTokenSource = null;
            }

            nftsCancellationTokenSource = new CancellationTokenSource();
         
            try
            {
                await DependencyService.Get<NftViewModel>().GetNFTsAsync(KeysModel.GetSubstrateKey(), nftsCancellationTokenSource.Token);
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


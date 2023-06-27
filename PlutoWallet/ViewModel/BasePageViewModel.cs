using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.View;

namespace PlutoWallet.ViewModel
{
	public partial class BasePageViewModel : ObservableObject
	{
        private LoadingView loadingView = new LoadingView();
        private MainView mainView = new MainView();
        private NftView nftView = new NftView();

        [ObservableProperty]
		private ContentView content;

		public BasePageViewModel()
		{
			content = mainView;
		}

        public void ReloadMainView()
        {
            mainView.Setup();
        }

        public void SetMainView()
		{
			Content = mainView;
            var networkViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();
            networkViewModel.SetupDefault();
        }

        public void SetNftView()
        {
            Content = nftView;
            NftViewModel nftViewModel = (NftViewModel)nftView.BindingContext;

            nftViewModel.GetNFTsAsync();
        }
    }
}


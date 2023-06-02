using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
		}

        public void SetNftView()
        {
            Content = nftView;
        }
    }
}


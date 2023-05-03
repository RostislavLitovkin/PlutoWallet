using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.Balance
{
	public partial class BalanceDashboardViewModel : ObservableObject
	{
        private const int EXTRA_HEIGHT = 35;

        [ObservableProperty]
        private double heightRequest;

        private ContentView content;
        public ContentView Content
        {
            get => content;
            set
            {
                SetProperty(ref content, value);
                HeightRequest = value.HeightRequest + EXTRA_HEIGHT;
            }
        }

        public BalanceDashboardViewModel()
		{
            // The name `Content` with capital C is on purpose
            Content = new UsdBalanceView();
		}
	}
}


using System;
using PlutoWallet.Constants;
using PlutoWallet.Components.NetworkSelect;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace PlutoWallet.ViewModel
{
    public class OptionsInfo
    {
        // Basically option indexes - I am just probably bad at naming things
        public int[] Groups { get; set; }
    }

	public partial class NetworkSelectionPageViewModel : ObservableObject
	{
        [ObservableProperty]
        private ObservableCollection<OptionsInfo> options = new ObservableCollection<OptionsInfo>();

        public NetworkSelectionPageViewModel()
		{
		}
	}
}


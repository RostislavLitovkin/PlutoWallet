using PlutoWallet.Constants;
using PlutoWallet.ViewModel;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlutoWallet.Components.Balance;
using System.Collections.ObjectModel;

namespace PlutoWallet.Components.NetworkSelect
{
	public partial class MultiNetworkSelectViewModel : ObservableObject
	{
        private const int MAX_BUBBLE_COUNT = 4;

        [ObservableProperty]
        private ObservableCollection<int> endpointIndexes = new ObservableCollection<int>();

        [ObservableProperty]
        private ObservableCollection<bool> showNames = new ObservableCollection<bool>();

        [ObservableProperty]
        private bool clicked;

        public MultiNetworkSelectViewModel()
        {
            SetupDefault();
        }

        public void SetupDefault()
        {
            int[] defaultNetworks = Endpoints.DefaultNetworks;

            EndpointIndexes = new ObservableCollection<int>();
            ShowNames = new ObservableCollection<bool>();

            for (int i = 0; i < MAX_BUBBLE_COUNT; i++)
            {
                EndpointIndexes[i] = Preferences.Get("SelectedNetworks" + i, defaultNetworks[i]);
                ShowNames[i] = false;
            }

            ShowNames[1] = true;

            // Update other views
            Task changeChain = Model.AjunaClientModel.ChangeChainGroupAsync(EndpointIndexes.ToArray());
        }
    }
}


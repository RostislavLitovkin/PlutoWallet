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
            SetupDefault();
		}

		private void SetupDefault()
		{
            var networkGroupOptions = new ObservableCollection<OptionsInfo>();

            var defaultNetworks = Endpoints.DefaultNetworks;

            // The selected option
            var selectedOptions = new int[4] {
                Preferences.Get("SelectedNetworks0", defaultNetworks[0]),
                Preferences.Get("SelectedNetworks1", defaultNetworks[1]),
                Preferences.Get("SelectedNetworks2", defaultNetworks[2]),
                Preferences.Get("SelectedNetworks3", defaultNetworks[3]),
            };
            

            networkGroupOptions.Add(new OptionsInfo
            {
               Groups = selectedOptions,
            });

            foreach (int[] option in Endpoints.NetworkOptions)
            {
                // Check if it is already selected
                bool selected = true;
                for (int i = 0; i < option.Length; i++)
                {
                    if (selectedOptions[i] != option[i])
                    {
                        selected = false;
                    }
                }

                if (!selected)
                {
                    networkGroupOptions.Add(new OptionsInfo
                    {
                        Groups = option,
                    });
                }
            }

            Options = networkGroupOptions;
        }
	}
}


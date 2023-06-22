using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.PredefinedLayouts
{
	public class EndpointIcon
	{
		public string Icon { get; set; }
	}

	public partial class GenericPredefinedLayoutItemViewModel : ObservableObject
	{
        [ObservableProperty]
        private ObservableCollection<EndpointIcon> endpointIcons = new ObservableCollection<EndpointIcon>();

        public GenericPredefinedLayoutItemViewModel()
		{
		}
	}
}


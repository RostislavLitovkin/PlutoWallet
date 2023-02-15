using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.ArgumentsView
{
	public partial class ArgumentsViewModel : ObservableObject
	{

        [ObservableProperty]
        private ObservableCollection<ArgProperties> args = new ObservableCollection<ArgProperties>();

        public ArgumentsViewModel()
		{

        }
    }

    public partial class ArgProperties : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string type;
    }
}


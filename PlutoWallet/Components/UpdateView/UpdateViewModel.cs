using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;

namespace PlutoWallet.Components.UpdateView
{
	public partial class UpdateViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool isVisible;

		public UpdateViewModel()
		{
			isVisible = false;
		}

		public async Task CheckLatestVersionAsync()
		{
            int currentBuild = int.Parse(AppInfo.Current.BuildString);

			var plutoWalletVersion = await VersionModel.GetPlutoWalletLatestVersionAsync();

			if (plutoWalletVersion is null)
			{
				return;
			}

			Console.WriteLine("Versions: " + plutoWalletVersion.Version + " > " + currentBuild);

            IsVisible = plutoWalletVersion.Version > currentBuild;
        }
    }
}


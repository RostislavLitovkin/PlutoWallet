using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.GalaxyLogicGame
{
	public partial class GLGPowerupsViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<GLGPowerupInfo> powerups = new ObservableCollection<GLGPowerupInfo>();

		public GLGPowerupsViewModel()
		{
			powerups = new ObservableCollection<GLGPowerupInfo>
			{
				new GLGPowerupInfo
				{
					Name = "Atomic bomb",
					Icon = "atomicbombshiny.png",
                },
				new GLGPowerupInfo
				{
					Name = "Extra plus",
					Icon = "extraplusshiny.png",
                } 
			};
		}
	}

	public class GLGPowerupInfo
	{
		public string Name { get; set; }
        public string Icon { get; set; }
    }
}


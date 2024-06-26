using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.Extrinsic
{

	public partial class ExtrinsicStatusStackViewModel : ObservableObject
	{
		[ObservableProperty]
		private Dictionary<string, ExtrinsicInfo> extrinsics = new Dictionary<string, ExtrinsicInfo>();

        [ObservableProperty]
        private ObservableCollection<ExtrinsicInfo> extrinsicInfos = new ObservableCollection<ExtrinsicInfo>();

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private int heightRequest;

        public ExtrinsicStatusStackViewModel()
		{
            isVisible = false;
        }

        public void Update()
        {
            ExtrinsicInfos = new ObservableCollection<ExtrinsicInfo>(Extrinsics.Values);
            IsVisible = ExtrinsicInfos.Any();

            HeightRequest = 75 * ExtrinsicInfos.Count() - 15;
        }
	}
}


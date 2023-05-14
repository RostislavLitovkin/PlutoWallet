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

        public ExtrinsicStatusStackViewModel()
		{
            /*
			var temp = new Dictionary<string, ExtrinsicInfo>();
			
            
			temp.Add("1", new ExtrinsicInfo
			{
                Status = ExtrinsicStatusEnum.Success,
                ExtrinsicId = "YYYY-YYYY TEST ID",
            });

            temp.Add("2", new ExtrinsicInfo
            {
				ExtrinsicId = "XXXX-XXXX TEST ID",
                Status = ExtrinsicStatusEnum.Pending,
            });

            temp.Add("3", new ExtrinsicInfo
            {
                Status = ExtrinsicStatusEnum.InBlock,
                ExtrinsicId = "XXXX-XXXX TEST ID",
            });

            temp.Add("4", new ExtrinsicInfo
            {
                Status = ExtrinsicStatusEnum.Pending,
                ExtrinsicId = "XXXX-XXXX TEST ID",
            });
            Extrinsics = temp;

            */

            isVisible = false;

        }

        public void Update()
        {
            ExtrinsicInfos = new ObservableCollection<ExtrinsicInfo>(Extrinsics.Values);
            IsVisible = ExtrinsicInfos.Any();
        }

		public void Remove(string id)
		{

		}
	}
}


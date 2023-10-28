using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.Nft
{
    /// 
    /// This code is by Klárka <3
    /// 
	public partial class NftLoadingViewModel : ObservableObject
	{
        private bool isVisible;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (value && !isVisible)
                {
                    SetProperty(ref isVisible, value);
                    StartTailLoop();
                }
                else
                {
                    SetProperty(ref isVisible, value);
                }


                
            }
        }

		[ObservableProperty]
		private bool tail1IsVisible;

        [ObservableProperty]
        private bool tail2IsVisible;

        [ObservableProperty]
        private bool tail3IsVisible;

        [ObservableProperty]
        private bool tail4IsVisible;

        [ObservableProperty]
        private bool tail5IsVisible;

        public NftLoadingViewModel()
		{
            tail2IsVisible = true;
            isVisible = false;
		}

        private async void StartTailLoop()
        {
            int count = 0;
            while (IsVisible)
            {
                await Task.Delay(100);
                count++;
                if (count == 1)
                {
                    Tail2IsVisible = false;
                    Tail1IsVisible = true;
                }
                else if (count == 2)
                {
                    Tail1IsVisible = false;
                    Tail2IsVisible = true;
                }
                else if (count == 3)
                {
                    Tail2IsVisible = false;
                    Tail3IsVisible = true;
                }
                else if (count == 4)
                {
                    Tail3IsVisible = false;
                    Tail4IsVisible = true;
                }
                else if (count == 5)
                {
                    Tail4IsVisible = false;
                    Tail5IsVisible = true;
                }
                else if (count == 6)
                {
                    Tail5IsVisible = false;
                    Tail4IsVisible = true;
                }
                else if (count == 7)
                {
                    Tail4IsVisible = false;
                    Tail3IsVisible = true;
                }
                else if (count == 8)
                {
                    Tail3IsVisible = false;
                    Tail2IsVisible = true;
                    count = 0;
                }
            }
        }
    }
}


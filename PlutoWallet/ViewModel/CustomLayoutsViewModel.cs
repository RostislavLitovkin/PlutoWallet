using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;

namespace PlutoWallet.ViewModel
{
	public partial class CustomLayoutsViewModel : ObservableObject
    {
		[ObservableProperty]
        private ObservableCollection<LayoutItemInfo> layoutItemInfos = new ObservableCollection<LayoutItemInfo>();

        public CustomLayoutsViewModel()
		{
            layoutItemInfos = Model.CustomLayoutModel.ParsePlutoLayoutItemInfos(
                Preferences.Get("PlutoLayout",
                Model.CustomLayoutModel.DEFAULT_PLUTO_LAYOUT)
            );
        }

        public void SwapItems(int originalIndex, int newIndex)
        {
            if (originalIndex == newIndex) return;

            var infos = new ObservableCollection<LayoutItemInfo>();

            if (originalIndex < newIndex)
            {
                newIndex++;
            }

            for (int i = 0; i < LayoutItemInfos.Count(); i++)
            {
                if (i == originalIndex)
                {
                    continue;
                }

                if (i == newIndex)
                {
                    infos.Add(LayoutItemInfos[originalIndex]);
                }

                infos.Add(LayoutItemInfos[i]);
            }

            if (newIndex == LayoutItemInfos.Count())
            {
                infos.Add(LayoutItemInfos[originalIndex]);
            }

            LayoutItemInfos = infos;

            Model.CustomLayoutModel.SaveLayout(infos);

            // reload the main view
            var basePageViewModel = DependencyService.Get<BasePageViewModel>();
            // This is broken unfortunately :(
            //basePageViewModel.ReloadMainView();
        }
	}
}


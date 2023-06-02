using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using PlutoWallet.Model;

namespace PlutoWallet.ViewModel
{
	public partial class AddCustomItemViewModel : ObservableObject
	{
        [ObservableProperty]
        private ObservableCollection<LayoutItemInfo> layoutItemInfos = new ObservableCollection<LayoutItemInfo>();

        public AddCustomItemViewModel()
        {
            try
            {
                var selectedItemInfos = Model.CustomLayoutModel.ParsePlutoLayoutItemInfos(
                    Preferences.Get("PlutoLayout",
                    Model.CustomLayoutModel.DEFAULT_PLUTO_LAYOUT)
                );

                var allItemInfos = Model.CustomLayoutModel.ParsePlutoLayoutItemInfos(Model.CustomLayoutModel.ALL_ITEMS);

                for(int i = 0; i < allItemInfos.Count(); i++)
                {
                    foreach(var selectedItem in selectedItemInfos)
                    {

                        if (allItemInfos[i].PlutoLayoutId == selectedItem.PlutoLayoutId)
                        {
                            Console.WriteLine("Removed: " + allItemInfos[i].PlutoLayoutId);
                            allItemInfos.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }

                layoutItemInfos = allItemInfos;
            }
            catch
            {
                Console.WriteLine("Layout Error");
                layoutItemInfos = Model.CustomLayoutModel.ParsePlutoLayoutItemInfos(Model.CustomLayoutModel.ALL_ITEMS);
            }
        }
	}
}


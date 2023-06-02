using PlutoWallet.Components.CustomLayouts;
using PlutoWallet.ViewModel;

namespace PlutoWallet.View;

public partial class AddCustomItemPage : ContentPage
{
	private CustomLayoutsViewModel customLayoutsViewModel;


    public AddCustomItemPage(CustomLayoutsViewModel customLayoutsViewModel)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        this.customLayoutsViewModel = customLayoutsViewModel;
    }

    private async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		Model.CustomLayoutModel.AddItemToSavedLayout(((CustomLayoutItemAddView)sender).PlutoLayoutId);

        customLayoutsViewModel.LayoutItemInfos = Model.CustomLayoutModel.ParsePlutoLayoutItemInfos(
                    Preferences.Get("PlutoLayout",
                    Model.CustomLayoutModel.DEFAULT_PLUTO_LAYOUT)
                );

        await Navigation.PopAsync();
    }
}

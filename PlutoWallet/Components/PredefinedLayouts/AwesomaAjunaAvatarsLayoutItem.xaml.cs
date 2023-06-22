using System.Collections.ObjectModel;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.PredefinedLayouts;

public partial class AwesomaAjunaAvatarsLayoutItem : ContentView
{
	private const string LAYOUT = "plutolayout: [dApp, ExSL, UsdB, ChaK, AAASeasonCountdown, AAALeaderboard];[4, 5]";


    public AwesomaAjunaAvatarsLayoutItem()
	{
		InitializeComponent();
    }

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Model.CustomLayoutModel.SaveLayout(LAYOUT);

        await Navigation.PopAsync();
    }
}

using System.Collections.ObjectModel;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.PredefinedLayouts;

public partial class AwesomaAjunaAvatarsLayoutItem : ContentView
{
	public const string LAYOUT = "plutolayout: [U, dApp, ExSL, UsdB, ChaK, AAALeaderboard];[bajun, ajuna]";

    // EXTRA: AAASeasonCountdown,

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

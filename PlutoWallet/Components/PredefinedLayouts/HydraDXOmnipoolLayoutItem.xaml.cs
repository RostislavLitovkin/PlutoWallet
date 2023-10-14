namespace PlutoWallet.Components.PredefinedLayouts;

public partial class HydraDXOmnipoolLayoutItem : ContentView
{
    private const string LAYOUT = "plutolayout: [dApp, ExSL, UsdB, ChaK, HDXOmniLiquidity, HDXDCA];[21]";

    public HydraDXOmnipoolLayoutItem()
	{
		InitializeComponent();
	}

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Model.CustomLayoutModel.SaveLayout(LAYOUT);

        await Navigation.PopAsync();
    }
}

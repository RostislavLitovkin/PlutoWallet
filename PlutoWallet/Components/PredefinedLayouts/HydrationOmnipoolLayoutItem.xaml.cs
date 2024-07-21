namespace PlutoWallet.Components.PredefinedLayouts;

public partial class HydrationOmnipoolLayoutItem : ContentView
{
    public const string LAYOUT = "plutolayout: [U, dApp, ExSL, UsdB, ChaK, HDXOmniLiquidity, HDXDCA];[Hydration]";

    public HydrationOmnipoolLayoutItem()
	{
		InitializeComponent();
	}

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Model.CustomLayoutModel.SaveLayout(LAYOUT);

        await Navigation.PopAsync();
    }
}

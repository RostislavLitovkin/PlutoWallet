namespace PlutoWallet.Components.PredefinedLayouts;

public partial class GalaxyLogicGameLayoutItem : ContentView
{
    public const string LAYOUT = "plutolayout: [U, dApp, ExSL, UsdB, ChaK, GLGPowerups];[Polkadot]";

    public GalaxyLogicGameLayoutItem()
    {
        InitializeComponent();
    }

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Model.CustomLayoutModel.SaveLayout(LAYOUT);

        await Navigation.PopAsync();
    }
}

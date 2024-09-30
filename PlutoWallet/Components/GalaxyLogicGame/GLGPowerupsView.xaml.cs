using PlutoWallet.Model;

namespace PlutoWallet.Components.GalaxyLogicGame;

public partial class GLGPowerupsView : ContentView
{
	public GLGPowerupsView()
	{
		InitializeComponent();

        this.SizeChanged += Load;
    }

	public void Load(object sender, EventArgs e)
    {
        powerupsLayout.WidthRequest = absoluteLayout.Width;
        powerupsLayout.Margin = new Thickness(0, watchImage.Height - 90, 0, 10);
    }
}

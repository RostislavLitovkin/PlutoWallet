namespace PlutoWallet.Components.Balance;

public partial class UsdBalanceView : ContentView
{
	public UsdBalanceView()
	{
		InitializeComponent();
        valueGraphSwitch.FirstMethod = ShowValue;
        valueGraphSwitch.SecondMethod = ShowGraph;
    }

    public bool ShowValue()
    {
        graphLayout.IsVisible = false;
        valueLayout.IsVisible = true;

        this.HeightRequest = 95;

        return true;
    }

    public bool ShowGraph()
    {
        graphLayout.IsVisible = true;
        valueLayout.IsVisible = false;

        
        this.HeightRequest = 635;

        return true;
    }
}

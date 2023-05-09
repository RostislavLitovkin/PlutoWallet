using PlutoWallet.Constants;

namespace PlutoWallet.Components.NetworkSelect;

public partial class MultiNetworkSelectView : ContentView
{
	public MultiNetworkSelectView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<MultiNetworkSelectViewModel>();
    }

    public bool Clicked { get; set; } = false;

    void OnOtherNetworksClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        if (Clicked)
        {
            return;
        }

        Clicked = true;
        ((AbsoluteLayout)this.Parent).Children.Add(new MultiNetworkSelectOptionsView
        {
            MultiSelect = this,
        });
        Clicked = false;
    }


}

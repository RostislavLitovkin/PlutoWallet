using PlutoWallet.Constants;

namespace PlutoWallet.Components.NetworkSelect;

public partial class NetworkSelectView : ContentView
{
    public NetworkSelectView()
    {
        BindingContext = DependencyService.Get<NetworkSelectViewModel>();

        InitializeComponent();
    }
}

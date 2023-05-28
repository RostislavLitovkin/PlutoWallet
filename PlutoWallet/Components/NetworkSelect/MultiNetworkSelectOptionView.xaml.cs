using Microsoft.Maui.Controls;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.NetworkSelect;

public partial class MultiNetworkSelectOptionView : ContentView
{
    public static readonly BindableProperty NetworksProperty = BindableProperty.Create(
        nameof(Networks), typeof(int[]), typeof(MultiNetworkSelectOptionView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (MultiNetworkSelectOptionView)bindable;

            control.networks = (int[])newValue;
            for (int i = 0; i < control.bubbles.Length; i++)
            {
                if (control.networks[i] != -1)
                {
                    control.bubbles[i].NetworkName = Endpoints.GetAllEndpoints[control.networks[i]].Name;
                    control.bubbles[i].Icon = Endpoints.GetAllEndpoints[control.networks[i]].Icon;
                    control.bubbles[i].IsVisible = true;
                }
            }
        });

    private NetworkBubbleView[] bubbles;

    private int[] networks;
    public MultiNetworkSelectOptionView()
    {
        InitializeComponent();

        bubbles = new NetworkBubbleView[4];
        bubbles[0] = bubble1;
        bubbles[1] = bubble2;
        bubbles[2] = bubble3;
        bubbles[3] = bubble4;
    }

    public int[] Networks
    {
        get => (int[])GetValue(NetworksProperty);
        set => SetValue(NetworksProperty, value);
    }

    private async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Preferences.Set("SelectedNetworks0", networks[0]);
        Preferences.Set("SelectedNetworks1", networks[1]);
        Preferences.Set("SelectedNetworks2", networks[2]);
        Preferences.Set("SelectedNetworks3", networks[3]);

        var multiNetworkSelectViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();
        multiNetworkSelectViewModel.SetupDefault();

        await Navigation.PopAsync();
    }
}

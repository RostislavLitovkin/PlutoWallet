using PlutoWallet.Constants;

namespace PlutoWallet.Components.NetworkSelect;

public partial class NetworkSelectView : ContentView
{
    public BindableProperty SelectedEndpointProperty = BindableProperty.Create(
        nameof(SelectedEndpoint), typeof(Endpoint), typeof(NetworkSelectView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, newValue, oldValue) => {
            var control = (NetworkSelectView)bindable;
            //control.picker.UpdateSelectedIndex(newValue)
        });


    private NetworkSelectViewModel bind;
    public NetworkSelectView()
    {
        InitializeComponent();

        BindingContext = DependencyService.Get<NetworkSelectViewModel>();
    }
    //public static readonly BindableProperty SelectedEndpointProperty;

    public Endpoint SelectedEndpoint {
        get
        {
            return (Endpoint)GetValue(SelectedEndpointProperty);
            //return (Endpoint)picker.SelectedItem;
        }
        set
        {
            SetValue(SelectedEndpointProperty, value);
        }
    }

}

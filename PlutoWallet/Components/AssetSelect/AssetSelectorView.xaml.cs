using PlutoWallet.Constants;
using System.Numerics;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.NetworkSelect;
using System.Net;
using PlutoWallet.Components.TransferView;
using PlutoWallet.Types;

namespace PlutoWallet.Components.AssetSelect;

public partial class AssetSelectorView : ContentView
{
    public static readonly BindableProperty AmountProperty = BindableProperty.Create(
        nameof(Amount), typeof(double), typeof(AssetSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetSelectorView)bindable;

            control.amountLabel.Text = String.Format("{0:0.00}", (double)newValue);
        });

    public static readonly BindableProperty SymbolProperty = BindableProperty.Create(
        nameof(Symbol), typeof(string), typeof(AssetSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetSelectorView)bindable;

            control.symbolLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty ChainIconsProperty = BindableProperty.Create(
        nameof(ChainIcons), typeof((string, string)), typeof(AssetSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetSelectorView)bindable;

            (string, string) values = ((string, string))newValue;
            control.chainIcon.SetAppTheme<FileImageSource>(Image.SourceProperty, values.Item1, values.Item2); ;
        });

    public static readonly BindableProperty UsdValueProperty = BindableProperty.Create(
        nameof(UsdValue), typeof(double), typeof(AssetSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetSelectorView)bindable;

            control.usdLabel.Text = String.Format("{0:0.00}", (double)newValue) + " USD";
        });

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected), typeof(bool), typeof(AssetSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetSelectorView)bindable;

            if ((bool)newValue)
            {
                control.border.BackgroundColor = Color.FromHex("7aff7a");
            }
            else
            {
                control.border.SetAppThemeColor(Label.BackgroundColorProperty, Color.FromHex("fdfdfd"), Color.FromHex("000000"));
            }
        });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
       nameof(Endpoint), typeof(Endpoint), typeof(AssetSelectorView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) => {
       });

    public static readonly BindableProperty PalletProperty = BindableProperty.Create(
       nameof(Pallet), typeof(AssetPallet), typeof(AssetSelectorView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) => {
       });

    public static readonly BindableProperty AssetIdProperty = BindableProperty.Create(
       nameof(AssetId), typeof(BigInteger), typeof(AssetSelectorView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) => {
       });

    public static readonly BindableProperty DecimalsProperty = BindableProperty.Create(
       nameof(Decimals), typeof(int), typeof(AssetSelectorView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) => {
       });

    public AssetSelectorView()
	{
		InitializeComponent();
	}

    public double Amount
    {
        get => (double)GetValue(AmountProperty);

        set => SetValue(AmountProperty, value);
    }

    public string Symbol
    {
        get => (string)GetValue(SymbolProperty);

        set => SetValue(SymbolProperty, value);
    }

    public (string, string) ChainIcons
    {
        get => ((string, string))GetValue(ChainIconsProperty);

        set => SetValue(ChainIconsProperty, value);
    }

    public double UsdValue
    {
        get => (double)GetValue(UsdValueProperty);

        set => SetValue(UsdValueProperty, value);
    }

    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);

        set => SetValue(EndpointProperty, value);
    }

    public BigInteger AssetId
    {
        get => (BigInteger)GetValue(AssetIdProperty);

        set => SetValue(AssetIdProperty, value);
    }

    public AssetPallet Pallet
    {
        get => (AssetPallet)GetValue(PalletProperty);

        set => SetValue(PalletProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(UsdValueProperty);

        set => SetValue(UsdValueProperty, value);
    }

    public int Decimals
    {
        get => (int)GetValue(DecimalsProperty);

        set => SetValue(DecimalsProperty, value);
    }

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        
        // Hide the AssetSelectView
        var assetSelectViewModel = DependencyService.Get<AssetSelectViewModel>();

        assetSelectViewModel.IsVisible = false;
        
        var networkingViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();
        foreach (NetworkSelectInfo info in networkingViewModel.NetworkInfos)
        {
            if (info.Name == Endpoint.Name && !info.ShowName)
            {
                // Change the network if not selected
                // This line also updates the fee
                networkingViewModel.Select(Endpoint.Key);
            }
        }

        var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();
        assetSelectButtonViewModel.ChainIcon = Application.Current.UserAppTheme == AppTheme.Light ? Endpoint.Icon : Endpoint.DarkIcon;
        assetSelectButtonViewModel.Symbol = Symbol;
        assetSelectButtonViewModel.AssetId = AssetId;
        assetSelectButtonViewModel.Pallet = Pallet;
        assetSelectButtonViewModel.Endpoint = Endpoint;
        assetSelectButtonViewModel.Decimals = Decimals;

        // Update the fee
        var transferViewModel = DependencyService.Get<TransferViewModel>();
        await transferViewModel.GetFeeAsync();
    }
}

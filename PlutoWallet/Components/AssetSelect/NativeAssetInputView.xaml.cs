using PlutoWallet.Components.Nft;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.AssetSelect;

public partial class NativeAssetInputView : ContentView
{
    public static readonly BindableProperty CardWidthProperty = BindableProperty.Create(
       nameof(CardWidth), typeof(int), typeof(NativeAssetInputView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) => {
           var control = (NativeAssetInputView)bindable;

           var width = (int)newValue - 10;
           control.usdGrid.WidthRequest = width;
           control.amountGrid.WidthRequest = width;
       });

    public static readonly BindableProperty AmountProperty = BindableProperty.Create(
        nameof(Amount), typeof(string), typeof(NativeAssetInputView),
        defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
        nameof(Endpoint), typeof(Endpoint), typeof(NativeAssetInputView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NativeAssetInputView)bindable;

            var buttonWidth = ((Endpoint)newValue).Unit.Length * 13 + 50;

            control.currencyBorder.WidthRequest = buttonWidth;
            control.usdButton.WidthRequest = buttonWidth;

            control.unitLabel.Text = ((Endpoint)newValue).Unit;
            control.chainIcon.Source = ((Endpoint)newValue).Icon;

        });
    public NativeAssetInputView()
    {
        InitializeComponent();
    }

    public int CardWidth
    {
        get => (int)GetValue(CardWidthProperty);
        set => SetValue(CardWidthProperty, value);
    }

    public string Amount
    {
        get => (string)GetValue(AmountProperty);
        set => SetValue(AmountProperty, value);
    }

    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);
        set => SetValue(EndpointProperty, value);
    }   

    /// <summary>
    /// Prevents from gettng stuck in a recursive loop due to inacurate USD conversions
    /// </summary>
    private bool duplicateBlocker = false;

    private void AmountEntryChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != "Text")
        {
            return;
        }

        var sellViewModel = DependencyService.Get<NftSellViewModel>();

        if (sellViewModel.IsVisible)
        {
            sellViewModel.Amount = ((Entry)sender).Text;
        }

        Console.WriteLine("amount change: " + duplicateBlocker);

        if (duplicateBlocker)
        {
            duplicateBlocker = false;
            return;
        }

        duplicateBlocker = true;

        sellViewModel.CalculateUsdValue(Endpoint.Unit);


        /// 2 way binding did not work for some reason
        //Amount = ((Entry)sender).Text;
    }

    private void UsdAmountEntryChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != "Text")
        {
            return;
        }

        Console.WriteLine("USD change: " + duplicateBlocker);

        if (duplicateBlocker)
        {
            duplicateBlocker = false;
            return;
        }

        duplicateBlocker = true;

        var sellViewModel = DependencyService.Get<NftSellViewModel>();

        sellViewModel.CalculateCurrencyAmount();
    }
}
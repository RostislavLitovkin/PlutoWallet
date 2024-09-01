
using PlutoWallet.Components.TransferView;

namespace PlutoWallet.Components.AssetSelect;

public partial class AssetInputView : ContentView
{
    public static readonly BindableProperty CardWidthProperty = BindableProperty.Create(
        nameof(CardWidth), typeof(int), typeof(AssetInputView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetInputView)bindable;

            var width = (int)newValue - 10;
            control.usdGrid.WidthRequest = width;
            control.amountGrid.WidthRequest = width;
        });

    public static readonly BindableProperty AmountProperty = BindableProperty.Create(
        nameof(Amount), typeof(string), typeof(AssetInputView),
        defaultBindingMode: BindingMode.TwoWay);


    public AssetInputView()
    {
        InitializeComponent();

        BindingContext = DependencyService.Get<AssetInputViewModel>();
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

        var transferViewModel = DependencyService.Get<TransferViewModel>();

        if (transferViewModel.IsVisible)
        {
            transferViewModel.Amount = ((Entry)sender).Text;
        }

        Console.WriteLine("amount change: " + duplicateBlocker);

        if (duplicateBlocker)
        {
            duplicateBlocker = false;
            return;
        }

        duplicateBlocker = true;

        var viewModel = DependencyService.Get<AssetInputViewModel>();

        viewModel.CalculateUsdValue();


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

        var viewModel = DependencyService.Get<AssetInputViewModel>();

        viewModel.CalculateCurrencyAmount();
    }
}
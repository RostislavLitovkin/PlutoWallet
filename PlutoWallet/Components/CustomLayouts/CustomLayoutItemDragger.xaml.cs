namespace PlutoWallet.Components.CustomLayouts;

public partial class CustomLayoutItemDragger : ContentView
{
    public static readonly BindableProperty ItemNameProperty = BindableProperty.Create(
        nameof(ItemName), typeof(string), typeof(CustomLayoutItemDragger),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (CustomLayoutItemDragger)bindable;
            control.nameLabelText.Text = (string)newValue;
        });

    public static readonly BindableProperty PlutoLayoutIdProperty = BindableProperty.Create(
        nameof(PlutoLayoutId), typeof(string), typeof(CustomLayoutItemDragger),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {

        });

    public CustomLayoutItemDragger()
	{
		InitializeComponent();
	}

    public string ItemName
    {
        get => (string)GetValue(ItemNameProperty);

        set => SetValue(ItemNameProperty, value);
    }

    public string PlutoLayoutId
    {
        get => (string)GetValue(PlutoLayoutIdProperty);

        set => SetValue(PlutoLayoutIdProperty, value);
    }

    
    private void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var customItemViewModel = DependencyService.Get<CustomItemViewModel>();

        customItemViewModel.Content = (ContentView)Model.CustomLayoutModel.GetItemPreview(PlutoLayoutId);

        customItemViewModel.ItemName = ItemName;

        customItemViewModel.IsVisible = true;

    }
}

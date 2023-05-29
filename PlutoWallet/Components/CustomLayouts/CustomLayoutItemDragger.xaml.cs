namespace PlutoWallet.Components.CustomLayouts;

public partial class CustomLayoutItemDragger : ContentView
{
    public static readonly BindableProperty ItemNameProperty = BindableProperty.Create(
        nameof(ItemName), typeof(string), typeof(CustomLayoutItemDragger),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (CustomLayoutItemDragger)bindable;
            control.nameLabel.Text = (string)newValue;
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
}

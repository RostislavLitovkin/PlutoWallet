namespace PlutoWallet.Components.CustomLayouts;

public partial class CustomLayoutItemAddView : ContentView
{
    public static readonly BindableProperty ItemNameProperty = BindableProperty.Create(
        nameof(ItemName), typeof(string), typeof(CustomLayoutItemAddView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (CustomLayoutItemAddView)bindable;
            control.nameLabelText.Text = (string)newValue;
        });

    public static readonly BindableProperty PlutoLayoutIdProperty = BindableProperty.Create(
        nameof(PlutoLayoutId), typeof(string), typeof(CustomLayoutItemAddView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {

        });

    public CustomLayoutItemAddView()
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

}

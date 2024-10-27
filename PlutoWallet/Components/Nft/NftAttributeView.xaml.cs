namespace PlutoWallet.Components.Nft;

public partial class NftAttributeView : ContentView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name), typeof(string), typeof(NftAttributeView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftAttributeView)bindable;

            control.attributeNameLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(string), typeof(NftAttributeView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftAttributeView)bindable;

            control.attributeValueLabel.Text = (string)newValue;
        });
    public NftAttributeView()
    {
        InitializeComponent();
    }

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}
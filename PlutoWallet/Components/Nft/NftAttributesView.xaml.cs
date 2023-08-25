using Markdig;

namespace PlutoWallet.Components.Nft;

public partial class NftAttributesView : ContentView
{
    public static readonly BindableProperty AttributesProperty = BindableProperty.Create(
        nameof(Attributes), typeof(string[]), typeof(NftAttributesView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftAttributesView)bindable;

            string[] attributes = (string[])newValue;

            if (attributes.Length == 0)
            {
                // do nothing
            }
            else
            {

                foreach (string attribute in attributes)
                {
                    control.attributesLabel.Text += attribute + " ";
                }

                control.IsVisible = true;
            }
        });

    public NftAttributesView()
	{
		InitializeComponent();
	}

    public string[] Attributes
    {
        get => (string[])GetValue(AttributesProperty);

        set => SetValue(AttributesProperty, value);
    }
}

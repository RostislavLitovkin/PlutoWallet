using Markdig;

namespace PlutoWallet.Components.Nft;

public partial class NftDescriptionView : ContentView
{
    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
        nameof(Description), typeof(string), typeof(NftDescriptionView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftDescriptionView)bindable;

            control.descriptionLabel.Text = Markdown.ToHtml((string)newValue).Trim([' ', '\n', '\t']);
        });

    public NftDescriptionView()
	{
		InitializeComponent();
	}

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);

        set => SetValue(DescriptionProperty, value);
    }
}

namespace PlutoWallet.Components.Nft;

#pragma warning disable CA1416 // Validate platform compatibility

public partial class NftImageView : ContentView
{
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource), typeof(string), typeof(NftImageView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftImageView)bindable;

            control.image.Source = (string)newValue;
        });
    public NftImageView()
	{
		InitializeComponent();
	}
    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
    private async void OnDownloadClicked(object sender, TappedEventArgs e)
    {
        await Browser.Default.OpenAsync(new Uri(ImageSource), BrowserLaunchMode.SystemPreferred);
        //await FileModel.SaveImageAsync(ImageSource, "nft.png");
    }
    private async void OnExpandClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new NftImageFullScreenPage(ImageSource));
    }
}
#pragma warning restore CA1416 // Validate platform compatibility

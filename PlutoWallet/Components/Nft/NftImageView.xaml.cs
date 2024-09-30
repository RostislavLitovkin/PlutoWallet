using PlutoWallet.Model;
using System.Numerics;

namespace PlutoWallet.Components.Nft;

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
        await FileModel.SaveImageAsync(ImageSource, "nft.png");
    }
    private async void OnExpandClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new NftImageFullScreenPage(ImageSource));
    }
}
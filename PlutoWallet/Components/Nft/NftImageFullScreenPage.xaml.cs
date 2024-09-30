namespace PlutoWallet.Components.Nft;

public partial class NftImageFullScreenPage : ContentPage
{
	public NftImageFullScreenPage(string imageSource)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        Console.WriteLine("Image source received: ");
        Console.WriteLine(imageSource);

        image.Source = imageSource;
    }
}
using PlutoWallet.Model;
using System.Collections.ObjectModel;
using NftKey = (UniqueryPlus.NftTypeEnum, System.Numerics.BigInteger, System.Numerics.BigInteger);

namespace PlutoWallet.Components.Nft;

public partial class NftGalleryView : ContentView
{
    private static Dictionary<NftKey, NftWrapper> nftsDict = new Dictionary<NftKey, NftWrapper>();

    public static readonly BindableProperty NftsProperty = BindableProperty.Create(
        nameof(Nfts), typeof(ObservableCollection<NftWrapper>), typeof(NftGalleryView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftGalleryView)bindable;

            Console.WriteLine("Received new featured Nfts");

            BindableLayout.SetItemsSource(control.horizontalStackLayout, (ObservableCollection<NftWrapper>)newValue);

            /*try
            {
                var collection = (ObservableCollection<NftWrapper>)newValue;

                foreach (var nftWrapper in collection)
                {
                    if (nftWrapper.Key.HasValue && !nftsDict.ContainsKey(nftWrapper.Key.Value))
                    {
                        nftsDict.Add(nftWrapper.Key.Value, nftWrapper);

                        control.horizontalStackLayout.Children.Add(new NftPictureView
                        {
                            HeightRequest = 200,
                            WidthRequest = 200,
                            NftBase = nftWrapper.NftBase,
                            Favourite = nftWrapper.Favourite,
                            Endpoint = nftWrapper.Endpoint
                        });

                        Console.WriteLine("Just added featured nft: " + nftWrapper.NftBase.Metadata.Name);

                    }
                }

                Console.WriteLine("ItemsSource set: " + collection.Count());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Setting Gallery View failed:");
                Console.WriteLine(ex);
            }*/
        });

    public static readonly BindableProperty PlusIsVisibleProperty = BindableProperty.Create(
       nameof(PlusIsVisible), typeof(bool), typeof(NftGalleryView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) =>
       {
           var control = (NftGalleryView)bindable;

           control.plusButton.IsVisible = (bool)newValue;
       });

    public static readonly BindableProperty LoadingIsVisibleProperty = BindableProperty.Create(
       nameof(LoadingIsVisible), typeof(bool), typeof(NftGalleryView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) =>
       {
           var control = (NftGalleryView)bindable;

           control.loadingItem.IsVisible = (bool)newValue;
       });

    public static readonly BindableProperty ErrorIsVisibleProperty = BindableProperty.Create(
      nameof(ErrorIsVisible), typeof(bool), typeof(NftGalleryView),
      defaultBindingMode: BindingMode.TwoWay,
      propertyChanging: (bindable, oldValue, newValue) =>
      {
          var control = (NftGalleryView)bindable;

          control.errorItem.IsVisible = (bool)newValue;
      });
    public NftGalleryView()
    {
        InitializeComponent();

        //BindingContext = DependencyService.Get<NftGalleryViewModel>();
    }
    public ObservableCollection<NftWrapper> Nfts
    {
        get => (ObservableCollection<NftWrapper>)GetValue(NftsProperty);
        set => SetValue(NftsProperty, value);
    }

    public bool PlusIsVisible
    {
        get => (bool)GetValue(PlusIsVisibleProperty);
        set => SetValue(PlusIsVisibleProperty, value);
    }

    public bool LoadingIsVisible
    {
        get => (bool)GetValue(LoadingIsVisibleProperty);
        set => SetValue(LoadingIsVisibleProperty, value);
    }

    public bool ErrorIsVisible
    {
        get => (bool)GetValue(ErrorIsVisibleProperty);
        set => SetValue(ErrorIsVisibleProperty, value);
    }
    void OnPlusClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {

    }
}

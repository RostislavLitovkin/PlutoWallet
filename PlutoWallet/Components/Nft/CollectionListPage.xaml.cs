using PlutoWallet.Model;
using CollectionKey = (UniqueryPlus.NftTypeEnum, System.Numerics.BigInteger);

namespace PlutoWallet.Components.Nft;

public partial class CollectionListPage : ContentPage
{
    public CollectionListPage(BaseListViewModel<CollectionKey, CollectionWrapper> bindingContext)
    {
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        BindingContext = bindingContext;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await ((BaseListViewModel<CollectionKey, CollectionWrapper>)this.BindingContext).InitialLoadAsync(CancellationToken.None);
    }
}
using PlutoWallet.Components.Events;
using PlutoWallet.Components.WebView;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.Extrinsic;

public partial class ExtrinsicDetailPage : ContentPage
{
    private Endpoint endpoint;
    private string blockNumberExtrinsicIndex;
	public ExtrinsicDetailPage(EventsListViewModel eventsListModel, Endpoint endpoint, string blockNumberExtrinsicIndex)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        eventsListView.BindingContext = eventsListModel;

        this.endpoint  = endpoint;
        this.blockNumberExtrinsicIndex = blockNumberExtrinsicIndex;
    }

    private async void OnOpenSubscanClicked(object sender, TappedEventArgs e)
    {
        if (endpoint.SubscanChainName is null)
        {
            return;
        }

        await Navigation.PushAsync(new WebViewPage($"https://{endpoint.SubscanChainName}.{Constants.Subscan.SUBSCAN_URL}/extrinsic/{blockNumberExtrinsicIndex}"));
    }
}
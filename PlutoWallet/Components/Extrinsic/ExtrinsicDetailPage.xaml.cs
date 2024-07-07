using PlutoWallet.Components.Events;

namespace PlutoWallet.Components.Extrinsic;

public partial class ExtrinsicDetailPage : ContentPage
{
	public ExtrinsicDetailPage(EventsListViewModel eventsListModel)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        eventsListView.BindingContext = eventsListModel;
    }
}
using System.Collections.ObjectModel;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Constants;
using PlutoWallet.View;

namespace PlutoWallet.Components.NetworkSelect;

public partial class MultiNetworkSelectView : ContentView
{
	public MultiNetworkSelectView()
	{
        InitializeComponent();

        BindingContext = DependencyService.Get<MultiNetworkSelectViewModel>();
    }

    public bool Clicked { get; set; } = false;

    void OnNetworkClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        try
        {
            if (((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent.Parent).ShowName)
            {
                // Probably do nothing
            }
            else
            {
                var viewModel = DependencyService.Get<MultiNetworkSelectViewModel>();

                var senderBubble = ((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent.Parent);

                Console.WriteLine("Selecting " + senderBubble.EndpointKey);    
                viewModel.Select(senderBubble.EndpointKey);
            }
        }
        catch (Exception ex)
        {
            var messagePopup = DependencyService.Get<MessagePopupViewModel>();

            messagePopup.Title = "MultiNetworkSelectView Error";
            messagePopup.Text = ex.ToString();

            messagePopup.IsVisible = true;
        }
    }

    async void OnOtherNetworksClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        if (Clicked)
        {
            return;
        }

        Clicked = true;
        var popupViewModel = DependencyService.Get<NetworkSelectPopupViewModel>();

        popupViewModel.SetNetworks();

        Clicked = false;
    }
}

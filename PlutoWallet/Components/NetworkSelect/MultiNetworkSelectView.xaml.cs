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
            var viewModel = DependencyService.Get<MultiNetworkSelectViewModel>();

            if (((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent.Parent).ShowName)
            {
                // Probably do nothing
            }
            else
            {
                var tempOldValues = viewModel.NetworkInfos;
                var networkInfos = new ObservableCollection<NetworkSelectInfo>();
                var endpointKeys = new List<EndpointEnum>();
                for (int i = 0; i < tempOldValues.Count; i++)
                {
                    networkInfos.Add(new NetworkSelectInfo {
                        ShowName = false,
                        Name = tempOldValues[i].Name,
                        Icon = tempOldValues[i].Icon,
                        EndpointKey = tempOldValues[i].EndpointKey,
                        DarkIcon = tempOldValues[i].DarkIcon,
                        EndpointConnectionStatus = tempOldValues[i].EndpointConnectionStatus,
                    });
                    endpointKeys.Add(networkInfos[i].EndpointKey);
                }

                var senderBubble = ((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent.Parent);

                int thisBubbleIndex = Array.IndexOf(endpointKeys.ToArray(), senderBubble.EndpointKey);
                networkInfos[thisBubbleIndex].ShowName = true;

                viewModel.NetworkInfos = networkInfos;

                // Update other views
                Task changeChain = Model.AjunaClientModel.ChangeChainAsync(senderBubble.EndpointKey);
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

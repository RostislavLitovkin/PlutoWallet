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

            if (((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent).ShowName)
            {
                // Probably do nothing
            }
            else
            {
                var tempOldValues = viewModel.NetworkInfos;
                var networkInfos = new ObservableCollection<NetworkSelectInfo>();
                var endpointIndexes = new List<int>();
                for (int i = 0; i < tempOldValues.Count; i++)
                {
                    networkInfos.Add(new NetworkSelectInfo {
                        ShowName = false,
                        Name = tempOldValues[i].Name,
                        Icon = tempOldValues[i].Icon,
                        EndpointIndex = tempOldValues[i].EndpointIndex,
                    });
                    endpointIndexes.Add(networkInfos[i].EndpointIndex);

                }

                var senderBubble = ((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent);



                int thisBubbleIndex = Array.IndexOf(endpointIndexes.ToArray(), senderBubble.EndpointIndex);
                networkInfos[thisBubbleIndex].ShowName = true;

                viewModel.NetworkInfos = networkInfos;

                // Update other views
                Task changeChain = Model.AjunaClientModel.ChangeChainAsync(senderBubble.EndpointIndex);
            }
        }
        catch (Exception ex)
        {
            var messagePopup = DependencyService.Get<MessagePopupViewModel>();

            messagePopup.Title = "Error";
            messagePopup.Text = ex.Message;

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
        await Navigation.PushAsync(new NetworkSelectionPage());
        Clicked = false;
    }


}

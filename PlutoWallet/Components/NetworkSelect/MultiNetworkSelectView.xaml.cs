using PlutoWallet.Constants;

namespace PlutoWallet.Components.NetworkSelect;

public partial class MultiNetworkSelectView : ContentView
{
    private NetworkBubbleView[] bubbles;

    

	public MultiNetworkSelectView()
	{
		InitializeComponent();

        bubbles = new NetworkBubbleView[4];
        bubbles[0] = bubble1;
        bubbles[1] = bubble2;
        bubbles[2] = bubble3;
        bubbles[3] = bubble4;

        SetupDefault();
    }

    public bool Clicked { get; set; } = false;

    public void SetupDefault()
    {
        int[] defaultNetworks = Endpoints.DefaultNetworks;

        List<int> selectedNetworkGroup = new List<int>(4);

        for (int i = 0; i < bubbles.Length; i++)
        {
            if (Preferences.Get("SelectedNetworks" + i, defaultNetworks[i]) != -1)
            {
                bubbles[i].Name = Endpoints.GetAllEndpoints[Preferences.Get("SelectedNetworks" + i, defaultNetworks[i])].Name;
                bubbles[i].Icon = Endpoints.GetAllEndpoints[Preferences.Get("SelectedNetworks" + i, defaultNetworks[i])].Icon;
                bubbles[i].EndpointIndex = Preferences.Get("SelectedNetworks" + i, defaultNetworks[i]);
                bubbles[i].IsVisible = true;

                selectedNetworkGroup.Add(Preferences.Get("SelectedNetworks" + i, defaultNetworks[i]));
            }
            else
            {
                bubbles[i].IsVisible = false;
            }
        }

        foreach (NetworkBubbleView bubble in bubbles)
        {
            bubble.ShowName = false;
        }
        bubble1.ShowName = true;

        // Update other views
        Task changeChain = Model.AjunaClientModel.ChangeChainGroupAsync(selectedNetworkGroup.ToArray());
    }

    void OnNetworkClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		if (((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent).ShowName)
		{
			// Probably do nothing
		}
		else
		{
            foreach (NetworkBubbleView bubble in bubbles)
            {
                bubble.ShowName = false;
            }

            var senderBubble = ((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent);
            senderBubble.ShowName = true;

            // Update other views
            Task changeChain = Model.AjunaClientModel.ChangeChainAsync(senderBubble.EndpointIndex);
        }
    }

    void OnOtherNetworksClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        if (Clicked)
        {
            return;
        }

        Clicked = true;
        ((AbsoluteLayout)this.Parent).Children.Add(new MultiNetworkSelectOptionsView
        {
            MultiSelect = this,
        });
    }


}

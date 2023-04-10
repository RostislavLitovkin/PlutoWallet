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

    public void SetupDefault()
    {
        var defaultNetworks = Endpoints.DefaultNetworks;

        for (int i = 0; i < bubbles.Length; i++)
        {
            if (Preferences.Get("SelectedNetworks" + i, defaultNetworks[i]) != -1)
            {
                bubbles[i].Name = Endpoints.GetAllEndpoints[Preferences.Get("SelectedNetworks" + i, defaultNetworks[i])].Name;
                bubbles[i].Icon = Endpoints.GetAllEndpoints[Preferences.Get("SelectedNetworks" + i, defaultNetworks[i])].Icon;
                bubbles[i].IsVisible = true;
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
    }

    void OnNetworkClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		if (((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent).ShowName)
		{
			// Later switch to other networks
		}
		else
		{
            foreach (NetworkBubbleView bubble in bubbles)
            {
                bubble.ShowName = false;
            }

            ((NetworkBubbleView)((HorizontalStackLayout)sender).Parent.Parent).ShowName = true;
        }
    }

    void OnOtherNetworksClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        ((AbsoluteLayout)this.Parent).Children.Add(new MultiNetworkSelectOptionsView
        {
            MultiSelect = this,
        });
    }
}

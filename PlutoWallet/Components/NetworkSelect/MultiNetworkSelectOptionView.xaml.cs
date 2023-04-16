using Microsoft.Maui.Controls;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.NetworkSelect;

public partial class MultiNetworkSelectOptionView : ContentView
{
    private NetworkBubbleView[] bubbles;

    private int[] networks;
    public MultiNetworkSelectOptionView()
    {
        InitializeComponent();

        bubbles = new NetworkBubbleView[4];
        bubbles[0] = bubble1;
        bubbles[1] = bubble2;
        bubbles[2] = bubble3;
        bubbles[3] = bubble4;
    }

    public int[] Networks
    {
        get => networks;
        set
        {
            networks = value;
            for (int i = 0; i < bubbles.Length; i++)
            {
                if (networks[i] != -1)
                {
                    bubbles[i].Name = Endpoints.GetAllEndpoints[networks[i]].Name;
                    bubbles[i].Icon = Endpoints.GetAllEndpoints[networks[i]].Icon;
                    bubbles[i].IsVisible = true;
                }
            }
        }
    }

    public BoxView ViewUsedForTapGesture => tapGestureView;
}

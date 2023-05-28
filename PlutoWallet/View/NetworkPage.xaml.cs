using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Constants;

namespace PlutoWallet.View;

public partial class NetworkPage : ContentPage
{

    public NetworkPage()
	{
		InitializeComponent();

        foreach (Endpoint endpoint in Endpoints.GetAllEndpoints)
        {
            NetworkDragger dragger = new NetworkDragger
            {
                SetEndpoint = endpoint,
                SetUpdatePositionAsyncFunction = UpdatePositionAsync,
                HorizontalOptions = LayoutOptions.Center,
            };

            networks.Children.Add(dragger);
        }


	}

    public async Task UpdatePositionAsync(double x, double y, double width, double height, Endpoint endpoint, GestureStatus status)
    {
        x -= scrollView.ScrollX;
        y -= scrollView.ScrollY;

        if (status == GestureStatus.Started)
        {
            pannedBubble.NetworkName = endpoint.Name;
            pannedBubble.Icon = endpoint.Icon;

            pannedBubble.TranslationX = x;
            pannedBubble.TranslationY = y;

            pannedBubble.IsVisible = true;
        }
        if (status == GestureStatus.Running)
        {
            pannedBubble.TranslationX = x;
            pannedBubble.TranslationY = y;

            if (selectionLayout.X < x + width &&
                selectionLayout.X + selectionLayout.Width > x &&
                selectionLayout.Y < y + height &&
                selectionLayout.Y + selectionLayout.Height > y)
            {
                selectionLayout.BackgroundColor = Color.Parse("Green");
            }
            else
            {
                selectionLayout.BackgroundColor = Color.Parse("Gray");
            }
        }
        else if (status == GestureStatus.Completed)
        {
            selectionLayout.BackgroundColor = Color.Parse("Gray");
            await pannedBubble.TranslateTo(x, y, 500, Easing.CubicOut);
            pannedBubble.IsVisible = false;
            scrollView.IsEnabled = true;
        }
    }
}

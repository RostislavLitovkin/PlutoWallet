using PlutoWallet.Constants;

namespace PlutoWallet.Components.NetworkSelect;

public partial class NetworkDragger : ContentView
{
    public delegate Task UpdatePositionFunction(double x, double y, double width, double height, Endpoint endpoint, GestureStatus status);

    private UpdatePositionFunction updatePositionAsync;

    private Queue<(float x, float y)> _positions = new Queue<(float, float)>();

    private Endpoint endpoint;

    private float x;
    private float y;

    public NetworkDragger()
    {
        InitializeComponent();
    }

    public UpdatePositionFunction SetUpdatePositionAsyncFunction { set { updatePositionAsync = value; } }

    public Endpoint SetEndpoint { set
        {
            endpoint = value;
            bubble.NetworkName = value.Name;
            bubble.Icon = value.Icon;
        } }

    async void PanGestureRecognizer_PanUpdated(System.Object sender, Microsoft.Maui.Controls.PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Started)
        {
            bubble.IsVisible = false;
            _positions = new Queue<(float, float)>();
            x = 0;
            y = 0;
        }
        if (e.StatusType == GestureStatus.Running)
        {
            _positions.Enqueue(((float)(e.TotalX), (float)(e.TotalY)));
            if (_positions.Count > 10)
                _positions.Dequeue();

            x = _positions.Average(item => item.x);
            y = _positions.Average(item => item.y);
        }
        if (e.StatusType == GestureStatus.Completed)
        {
            x = 0;
            y = 0;
        }

        // do all the custom steps
        await updatePositionAsync(
            x + this.X,
            y + this.Y + 150,
            bubble.Width,
            bubble.Height,
            endpoint,
            e.StatusType);

        if (e.StatusType == GestureStatus.Completed)
        {
            bubble.IsVisible = true;
        }
    }
}

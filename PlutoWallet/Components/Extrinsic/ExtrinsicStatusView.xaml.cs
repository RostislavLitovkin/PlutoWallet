using PlutoWallet.Constants;
using PlutoWallet.Components.WebView;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Base;

namespace PlutoWallet.Components.Extrinsic;

public partial class ExtrinsicStatusView : ContentView
{

    private Queue<(float x, float y)> _positions = new Queue<(float, float)>();

    public static readonly BindableProperty ExtrinsicIdProperty = BindableProperty.Create(
        nameof(ExtrinsicId), typeof(string), typeof(ExtrinsicStatusView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ExtrinsicStatusView)bindable;
            control.nameLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty StatusProperty = BindableProperty.Create(
        nameof(Status), typeof(ExtrinsicStatusEnum), typeof(ExtrinsicStatusView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ExtrinsicStatusView)bindable;

            switch ((ExtrinsicStatusEnum)newValue)
            {
                case ExtrinsicStatusEnum.InBlock:
                    control.statusLabel.Text = "In block";
                    control.statusLabel.TextColor = Color.Parse("Orange");
                    break;
                case ExtrinsicStatusEnum.Pending:
                    Console.WriteLine("Pending");
                    control.statusLabel.Text = "Pending";
                    control.statusLabel.TextColor = Color.Parse("Orange");
                    break;
                case ExtrinsicStatusEnum.Failed:
                    control.statusLabel.Text = "Failed";
                    control.statusLabel.TextColor = Color.Parse("Red");
                    break;
                case ExtrinsicStatusEnum.Success:
                    control.statusLabel.Text = "Success";
                    control.statusLabel.TextColor = Color.Parse("Green");
                    break;
                default:
                    // Handle errors
                    break;
            }
        });

    public static readonly BindableProperty HashProperty = BindableProperty.Create(
        nameof(Hash), typeof(Hash), typeof(ExtrinsicStatusView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ExtrinsicStatusView)bindable;
        });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
        nameof(Endpoint), typeof(Endpoint), typeof(ExtrinsicStatusView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ExtrinsicStatusView)bindable;

            control.calamarButton.IsVisible = ((Endpoint)newValue).CalamarChainName != null;
        });

    public ExtrinsicStatusView()
	{
		InitializeComponent();
	}

    public string ExtrinsicId
    {
        get => (string)GetValue(ExtrinsicIdProperty);

        set => SetValue(ExtrinsicIdProperty, value);
    }

    public ExtrinsicStatusEnum Status
    {
        get => (ExtrinsicStatusEnum)GetValue(StatusProperty);

        set => SetValue(StatusProperty, value);
    }

    public Hash Hash
    {
        get => (Hash)GetValue(HashProperty);

        set => SetValue(HashProperty, value);
    }

    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);

        set => SetValue(EndpointProperty, value);
    }

    void OnRemoveClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var extrinsicStackViewModel = DependencyService.Get<ExtrinsicStatusStackViewModel>();

        extrinsicStackViewModel.Extrinsics.Remove(ExtrinsicId);

        extrinsicStackViewModel.Update();
    }

    async void OnCalamarClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        if(Endpoint.CalamarChainName != null)
        {
            await Navigation.PushAsync(new WebViewPage("https://calamar.app/" + Endpoint.CalamarChainName + "/search?query=" + Hash.Value));
        }
    }

    async void OnPanUpdated(System.Object sender, Microsoft.Maui.Controls.PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Started)
        {
            //protectiveLayout.IsVisible = true;

            _positions = new Queue<(float, float)>();
        }

        if (e.StatusType == GestureStatus.Running)
        {
            _positions.Enqueue(((float)(e.TotalX), (float)(e.TotalY)));
            if (_positions.Count > 10)
                _positions.Dequeue();

            card.TranslationX = _positions.Average(item => item.x);
        }

        if (e.StatusType == GestureStatus.Completed)
        {
          
            if (card.TranslationX < -50)
            {
                await Task.WhenAll(
                    card.TranslateTo(card.Width * -1 - 30, 0, 500, Easing.CubicIn)
                    //, this.ScaleYTo(0, 500)
                    );
            }
            else if (card.TranslationX > 50)
            {
                await Task.WhenAll(
                    card.TranslateTo(card.Width + 30, 0, 500, Easing.CubicIn)
                    //, this.ScaleYTo(0, 500)
                    );
            }
            else
            {
                await card.TranslateTo(0, 0, 500, Easing.CubicOut);

                return;
            }

            var extrinsicStackViewModel = DependencyService.Get<ExtrinsicStatusStackViewModel>();

            extrinsicStackViewModel.Extrinsics.Remove(ExtrinsicId);

            extrinsicStackViewModel.Update();

            //protectiveLayout.IsVisible = false;
        }
    }
}

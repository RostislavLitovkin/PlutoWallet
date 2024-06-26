using PlutoWallet.Constants;
using PlutoWallet.Components.WebView;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.ViewModel;

namespace PlutoWallet.Components.Extrinsic;

public partial class ExtrinsicStatusView : ContentView
{

    private Queue<(float x, float y)> _positions = new Queue<(float, float)>();

    public static readonly BindableProperty ExtrinsicIdProperty = BindableProperty.Create(
        nameof(ExtrinsicId), typeof(string), typeof(ExtrinsicStatusView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ExtrinsicStatusView)bindable;
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
                    control.statusLabel.TextColor = Colors.Green;
                    break;
                case ExtrinsicStatusEnum.Pending:
                    control.statusLabel.Text = "Pending";
                    control.statusLabel.TextColor = Colors.Orange;
                    break;
                case ExtrinsicStatusEnum.Submitting:
                    control.statusLabel.Text = "Submitting";
                    control.statusLabel.TextColor = Colors.Gray;
                    break;
                case ExtrinsicStatusEnum.Failed:
                    control.statusLabel.Text = "Failed";
                    control.statusLabel.TextColor = Colors.DarkRed;
                    break;
                case ExtrinsicStatusEnum.Finalized:
                    control.statusLabel.Text = "Finalized";
                    control.statusLabel.TextColor = Colors.Green;
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

            control.chainIcon.Source = ((Endpoint)newValue).Icon;
        });

    public static readonly BindableProperty CallNameProperty = BindableProperty.Create(
       nameof(CallName), typeof(string), typeof(ExtrinsicStatusView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) => {
           var control = (ExtrinsicStatusView)bindable;

           control.nameLabelText.Text = (string)newValue;

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

    public string CallName
    {
        get => (string)GetValue(CallNameProperty);

        set => SetValue(CallNameProperty, value);
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
            var mainViewModel = DependencyService.Get<MainViewModel>();

            mainViewModel.ScrollIsEnabled = false;

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

            var mainViewModel = DependencyService.Get<MainViewModel>();

            mainViewModel.ScrollIsEnabled = true;
        }
    }
}

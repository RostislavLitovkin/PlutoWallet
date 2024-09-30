using PlutoWallet.Constants;
using PlutoWallet.Components.WebView;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.ViewModel;
using PlutoWallet.Components.Events;
using CommunityToolkit.Maui.Converters;
using System.Numerics;

namespace PlutoWallet.Components.Extrinsic;

public partial class ExtrinsicStatusView : ContentView
{

    private bool clicked = false;

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
                case ExtrinsicStatusEnum.InBlockSuccess:
                    control.statusLabel.Text = "In block - Success";
                    control.statusLabel.TextColor = Colors.Green;
                    break;
                case ExtrinsicStatusEnum.InBlockFailed:
                    control.statusLabel.Text = "In block - Failed";
                    control.statusLabel.TextColor = Colors.DarkRed;
                    break;
                case ExtrinsicStatusEnum.FinalizedSuccess:
                    control.statusLabel.Text = "Finalized - Success";
                    control.statusLabel.TextColor = Colors.Green;
                    break;
                case ExtrinsicStatusEnum.FinalizedFailed:
                    control.statusLabel.Text = "Finalized - Failed";
                    control.statusLabel.TextColor = Colors.DarkRed;
                    break;
                case ExtrinsicStatusEnum.Pending:
                    control.statusLabel.Text = "Pending";
                    control.statusLabel.TextColor = Colors.Orange;
                    break;
                case ExtrinsicStatusEnum.Submitting:
                    control.statusLabel.Text = "Submitting";
                    control.statusLabel.TextColor = Colors.Gray;
                    break;
                case ExtrinsicStatusEnum.Dropped:
                    control.statusLabel.Text = "Dropped";
                    control.statusLabel.TextColor = Colors.DarkRed;
                    break;
                case ExtrinsicStatusEnum.Error:
                    control.statusLabel.Text = "Error";
                    control.statusLabel.TextColor = Colors.DarkRed;
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

            control.chainIcon.Source = ((Endpoint)newValue).Icon;
        });

    public static readonly BindableProperty CallNameProperty = BindableProperty.Create(
       nameof(CallName), typeof(string), typeof(ExtrinsicStatusView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) => {
           var control = (ExtrinsicStatusView)bindable;

           control.nameLabelText.Text = (string)newValue;

       });

    public static readonly BindableProperty EventsListViewModelProperty = BindableProperty.Create(
       nameof(EventsListViewModel), typeof(TaskCompletionSource<EventsListViewModel>), typeof(ExtrinsicStatusView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) => {
       });

    public static readonly BindableProperty BlockNumberProperty = BindableProperty.Create(
        nameof(BlockNumber), typeof(BigInteger), typeof(ExtrinsicStatusView),
        defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty ExtrinsicIndexProperty = BindableProperty.Create(
        nameof(ExtrinsicIndex), typeof(uint?), typeof(ExtrinsicStatusView),
        defaultBindingMode: BindingMode.TwoWay);

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

    public BigInteger BlockNumber
    {
        get => (BigInteger)GetValue(BlockNumberProperty);
        set => SetValue(BlockNumberProperty, value);
    }

    public uint? ExtrinsicIndex
    {
        get => (uint?)GetValue(ExtrinsicIndexProperty);
        set => SetValue(ExtrinsicIndexProperty, value);
    }

    public TaskCompletionSource<EventsListViewModel> EventsListViewModel
    {
        get => (TaskCompletionSource<EventsListViewModel>)GetValue(EventsListViewModelProperty);
        set => SetValue(EventsListViewModelProperty, value);
    }

    async void OnCloseClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var extrinsicStackViewModel = DependencyService.Get<ExtrinsicStatusStackViewModel>();

        extrinsicStackViewModel.Extrinsics.Remove(ExtrinsicId);

        extrinsicStackViewModel.Update();
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

    private async void OnClicked(object sender, TappedEventArgs e)
    {
        if (EventsListViewModel == null && clicked)
        {
            return;
        }

        clicked = true;

        await Navigation.PushAsync(new ExtrinsicDetailPage(
            await EventsListViewModel.Task,
            Endpoint,
            BlockNumber + "-" + ExtrinsicIndex
        ));

        clicked = false;
    }
}

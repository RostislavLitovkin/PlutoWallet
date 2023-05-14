namespace PlutoWallet.Components.Extrinsic;

public partial class ExtrinsicStatusView : ContentView
{
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

    void OnRemoveClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var extrinsicStackViewModel = DependencyService.Get<ExtrinsicStatusStackViewModel>();

        extrinsicStackViewModel.Extrinsics.Remove(ExtrinsicId);

        extrinsicStackViewModel.Update();
    }
}

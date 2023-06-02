namespace PlutoWallet.Components.Extrinsic;

public partial class ExtrinsicStatusStackLayout : ContentView
{
	public ExtrinsicStatusStackLayout()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ExtrinsicStatusStackViewModel>();
    }

    public ExtrinsicStatusStackLayout(ExtrinsicStatusStackViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    public ExtrinsicStatusStackLayout(ExtrinsicStatusStackViewModel viewModel, int heightRequest)
    {
        InitializeComponent();

        BindingContext = viewModel;

        this.HeightRequest = heightRequest;
    }
}

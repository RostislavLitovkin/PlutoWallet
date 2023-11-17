namespace PlutoWallet.Components.Referenda;

public partial class ReferendaView : ContentView
{
	public ReferendaView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ReferendaViewModel>();
    }

    public ReferendaView(ReferendaViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}

using Plutonication;

namespace PlutoWallet.Components.TransactionRequest;

public partial class TransactionRequestView : ContentView
{
	public TransactionRequestView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<TransactionRequestViewModel>();
    }
    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Model.PlutonicationModel.EventManager.SendMessageAsync(MessageCode.Refused);

        // Hide this layout
        var viewModel = DependencyService.Get<TransactionRequestViewModel>();
        viewModel.IsVisible = false;
    }

    async void OnSubmitClicked(System.Object sender, System.EventArgs e)
    {
        await Model.PlutonicationModel.EventManager.SendMessageAsync(MessageCode.Success);
    }

    async void OnRejectClicked(System.Object sender, System.EventArgs e)
    {
        await Model.PlutonicationModel.EventManager.SendMessageAsync(MessageCode.Refused);

        // Hide this layout
        var viewModel = DependencyService.Get<TransactionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}

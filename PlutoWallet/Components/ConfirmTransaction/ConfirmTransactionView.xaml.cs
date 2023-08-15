namespace PlutoWallet.Components.ConfirmTransaction;

public partial class ConfirmTransactionView : ContentView
{
	public ConfirmTransactionView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ConfirmTransactionViewModel>();
    }

    void OnVerifyClicked(System.Object sender, System.EventArgs e)
    {
        var viewModel = DependencyService.Get<ConfirmTransactionViewModel>();
        if (viewModel.Password == Preferences.Get("password", ""))
        {
            viewModel.Status = ConfirmTransactionStatus.Verified;

            // hide animation
            viewModel.IsVisible = false;
        }
        else
        {
            errorLabel.IsVisible = true;
        }
    }

    void OnPasswordChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        errorLabel.IsVisible = false;
    }

    void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<ConfirmTransactionViewModel>();
        viewModel.Status = ConfirmTransactionStatus.Denied;
        viewModel.IsVisible = false;
    }
}

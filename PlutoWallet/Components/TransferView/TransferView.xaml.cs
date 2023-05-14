using PlutoWallet.Model;
using PlutoWallet.Components.ScannerView;
using System.Numerics;

namespace PlutoWallet.Components.TransferView;

public partial class TransferView : ContentView
{
	public TransferView()
	{
        var viewModel = DependencyService.Get<TransferViewModel>();

        BindingContext = viewModel;

        InitializeComponent();

        //viewModel.GetFeeAsync();
    }

    async void SignAndTransferClicked(System.Object sender, System.EventArgs e)
    {
        // Send the actual transaction

        var viewModel = DependencyService.Get<TransferViewModel>();
        decimal tempAmount;
        BigInteger amount;
        if (decimal.TryParse(viewModel.Amount, out tempAmount))
        {
            // Double to int conversion
            // Complete later

            amount = (BigInteger)(tempAmount * (decimal)Math.Pow(10, Model.AjunaClientModel.SelectedEndpoint.Decimals));
        }
        else
        {
            errorLabel.Text = "Invalid amount value";
            return;
        }

        errorLabel.Text = "";

        try
        {

            await TransferModel.BalancesTransferAsync(viewModel.Address, amount);

            // Hide this layout

            viewModel.SetToDefault();
        }
        catch (Exception ex)
        {
            errorLabel.Text = ex.Message;
        }

        
    }

    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Hide this layout
        var viewModel = DependencyService.Get<TransferViewModel>();

        viewModel.SetToDefault();

        qrLayout.Children.Clear();
    }

    void OnShowQRClicked(System.Object sender, System.EventArgs e)
    {
        var scanner = new ScannerView.ScannerView
        {
            OnScannedMethod = OnScanned
        };

        qrLayout.Children.Add(scanner);
    }

    async void OnScanned(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var viewModel = DependencyService.Get<TransferViewModel>();

        try
        {
            viewModel.Address = e.Results[e.Results.Length - 1].Value;

            qrLayout.Children.Clear();
        }
        catch (Exception ex)
        {

        }
    }

}

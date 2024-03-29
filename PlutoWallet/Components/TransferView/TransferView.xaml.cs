﻿using PlutoWallet.Model;
using PlutoWallet.Components.ScannerView;

namespace PlutoWallet.Components.TransferView;

public partial class TransferView : ContentView
{
	public TransferView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<TransferViewModel>();
    }

    async void SignAndTransferClicked(System.Object sender, System.EventArgs e)
    {
        // Send the actual transaction

        var viewModel = DependencyService.Get<TransferViewModel>();

        try
        {


            await TransferModel.BalancesTransferAsync(viewModel.Address, viewModel.Amount);

            // Hide this layout

            viewModel.IsVisible = false;
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

        viewModel.IsVisible = false;

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

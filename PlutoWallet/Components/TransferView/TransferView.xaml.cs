using PlutoWallet.Model;
using PlutoWallet.Components.ScannerView;
using PlutoWallet.Components.AssetSelect;
using PlutoWallet.Components.Extrinsic;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
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

        var client = Model.AjunaClientModel.Client;

        try
        {
            
            var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

            Method transfer =
                assetSelectButtonViewModel.Pallet == Balance.AssetPallet.Native ?
                TransferModel.NativeTransfer(client, viewModel.Address, amount) :
                TransferModel.AssetsTransfer(client, viewModel.Address, assetSelectButtonViewModel.AssetId, amount);

            if ((await KeysModel.GetAccount()).IsSome(out var account))
            {
                UnCheckedExtrinsic extrinsic = await client.GetExtrinsicParametersAsync(
                    transfer,
                    account,
                    client.DefaultCharge,
                    lifeTime: 64,
                    signed: true,
                    CancellationToken.None);


                var extrinsicStackViewModel = DependencyService.Get<ExtrinsicStatusStackViewModel>();

                string extrinsicId = await client.Author.SubmitAndWatchExtrinsicAsync(
                    (string id, ExtrinsicStatus status) =>
                    {
                        if (status.ExtrinsicState == ExtrinsicState.Ready)
                            Console.WriteLine("Ready");
                        else if (status.ExtrinsicState == ExtrinsicState.Dropped)
                        {
                            extrinsicStackViewModel.Extrinsics[id].Status = ExtrinsicStatusEnum.Failed;
                            extrinsicStackViewModel.Update();
                        }

                        else if (status.InBlock != null)
                        {
                            Console.WriteLine("In block");
                            extrinsicStackViewModel.Extrinsics[id].Status = ExtrinsicStatusEnum.InBlock;
                            extrinsicStackViewModel.Update();
                        }

                        else if (status.Finalized != null)
                        {
                            Console.WriteLine("Finalized");
                            extrinsicStackViewModel.Extrinsics[id].Status = ExtrinsicStatusEnum.Success;
                            extrinsicStackViewModel.Update();
                        }

                        else
                            Console.WriteLine(status.ExtrinsicState);
                    },
                    Utils.Bytes2HexString(extrinsic.Encode()), CancellationToken.None);

                extrinsicStackViewModel.Extrinsics.Add(
                    extrinsicId,
                    new ExtrinsicInfo
                    {
                        ExtrinsicId = extrinsicId,
                        Status = ExtrinsicStatusEnum.Pending,
                    });

                extrinsicStackViewModel.Update();
            }
            else
            {
                // Verification failed, do something about it
            }

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

using System.Numerics;
using Microsoft.Maui.Controls.Shapes;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;

namespace PlutoWallet.Components.Xcm;

public partial class XcmTransferPage : ContentPage
{
	public XcmTransferPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        var viewModel = DependencyService.Get<XcmTransferViewModel>();

        BindingContext = viewModel;

        InitializeComponent();
	}

    async void OnSignAndTransferClicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            var viewModel = DependencyService.Get<XcmTransferViewModel>();

            var popupViewModel = DependencyService.Get<XcmNetworkSelectPopupViewModel>();

            var originEndpoint = Endpoints.GetEndpointDictionary[popupViewModel.OriginKey];

            string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(originEndpoint.URLs);

            PlutoWalletSubstrateClient clientExt = new PlutoWalletSubstrateClient(
                originEndpoint,
                new Uri(bestWebSecket),
                ChargeTransactionPayment.Default()
            );

            Console.WriteLine("Origin key: " + popupViewModel.OriginKey);
            Console.WriteLine("Destination key: " + popupViewModel.DestinationKey);

            await clientExt.ConnectAndLoadMetadataAsync();

            var client = clientExt.SubstrateClient;

            Method transferMethod = XcmTransferModel.XcmTransfer(
                 clientExt,
                 originEndpoint,
                 Endpoints.GetEndpointDictionary[popupViewModel.DestinationKey],
                 KeysModel.GetSubstrateKey(),

                 // TODO Add proper decimal management
                 (BigInteger)viewModel.Amount
            );

            Console.WriteLine(Utils.Bytes2HexString(transferMethod.Encode()).ToLower());

            // 0x630903000100a10f03000101006a4e76d530fa715a95388b889ad33c1665062c3dec9bf0aca3a9e4ff45781e480304000000000700743ba40b0000000000
            // 0x630903000100a10f03000101006a4e76d530fa715a95388b889ad33c1665062c3dec9bf0aca3a9e4ff45781e480304000000000700743ba40b0000000000

            if ((await KeysModel.GetAccount()).IsSome(out var account))
            {
                string extrinsicId = await clientExt.SubmitExtrinsicAsync(transferMethod, account, token: CancellationToken.None);


                Console.WriteLine("Extrinsic ID: " + extrinsicId);
            }
            else
            {
                // Verification failed, do something about it
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}

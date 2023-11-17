using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Chaos.NaCl;
using Schnorrkel;

namespace PlutoWallet.Components.Vault;

public partial class VaultSignView : ContentView
{
	public VaultSignView()
	{
		InitializeComponent();
        BindingContext = DependencyService.Get<VaultSignViewModel>();
    }

    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Hide this layout
        var viewModel = DependencyService.Get<VaultSignViewModel>();
        viewModel.IsVisible = false;
    }

    async void OnSignClicked(System.Object sender, System.EventArgs e)
    {
        if ((await Model.KeysModel.GetAccount()).IsSome(out var account))
        {
            var viewModel = DependencyService.Get<VaultSignViewModel>();

            string signature;
            switch (account.KeyType)
            {
                case KeyType.Ed25519:
                    signature = "00" + Utils.Bytes2HexString(Ed25519.Sign(viewModel.Payload, account.PrivateKey), Utils.HexStringFormat.Pure);
                    break;

                case KeyType.Sr25519:
                    signature = "01" + Utils.Bytes2HexString(Sr25519v091.SignSimple(account.Bytes, account.PrivateKey, viewModel.Payload), Utils.HexStringFormat.Pure);
                    break;

                default:
                    return;
            }

            viewModel.Signature = signature;
            viewModel.SignatureIsVisible = !true;
            viewModel.SignButtonIsVisible = false;

            Console.WriteLine(signature);
        }
    }

    async void OnRejectClicked(System.Object sender, System.EventArgs e)
    {
        // Hide this layout
        var viewModel = DependencyService.Get<VaultSignViewModel>();
        viewModel.IsVisible = false;
    }
}

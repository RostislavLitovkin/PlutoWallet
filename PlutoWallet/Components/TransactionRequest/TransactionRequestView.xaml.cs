
using Substrate.NetApi;
using Plutonication;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Rpc;
using System.Globalization;

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
        // Maybe send a refuse message 

        // Hide this layout
        var viewModel = DependencyService.Get<TransactionRequestViewModel>();
        viewModel.IsVisible = false;
    }

    // helper function
    public static uint HexStringToUint(string hex)
    {
        hex = hex.Replace("0x", ""); // remove the 0x if it's there
        if (uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint result))
        {
            return result;
        }
        else
        {
            throw new FormatException("The provided string is not a valid hexadecimal number");
        }
    }

    async void OnSubmitClicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            var viewModel = DependencyService.Get<TransactionRequestViewModel>();
            Plutonication.Payload payload = viewModel.Payload;

            Console.WriteLine("Payload received, nice!");
            Console.WriteLine(payload.method);

            byte[] methodBytes = Utils.HexToByteArray(payload.method);

            List<byte> methodParameters = new List<byte>();

            for (int i = 2; i < methodBytes.Length; i++)
            {
                methodParameters.Add(methodBytes[i]);
            }

            Method method = new Method(methodBytes[0], methodBytes[1], methodParameters.ToArray());

            Hash eraHash = new Hash();
            eraHash.Create(Utils.HexToByteArray(payload.era));

            Hash blockHash = new Hash();
            blockHash.Create(payload.blockHash);

            Console.WriteLine("HexEra: " + payload.era);
            Console.WriteLine(eraHash);

            Hash genesisHash = new Hash();
            genesisHash.Create(Utils.HexToByteArray(payload.genesisHash));

            RuntimeVersion runtime = new RuntimeVersion
            {
                ImplVersion = payload.version,
                SpecVersion = HexStringToUint(payload.specVersion),
                TransactionVersion = HexStringToUint(payload.transactionVersion),
            };

            if ((await Model.KeysModel.GetAccount()).IsSome(out var account))
            {
                var extrinsic = RequestGenerator.SubmitExtrinsic(
                    true,
                    account,
                    method,
                    Era.Decode(Utils.HexToByteArray(payload.era)),
                    HexStringToUint(payload.nonce),
                    new ChargeTransactionPayment(HexStringToUint(payload.tip)),
                    genesisHash,
                    blockHash,
                    runtime
                );

                var signerResult = new SignerResult
                {
                    id = 1,
                    signature = Utils.Bytes2HexString(new byte[1] { 1 }.Concat(extrinsic.Signature).ToArray()).ToLower(),
                };

                await PlutonicationWalletClient.SendSignedPayloadAsync(signerResult);
            }

            // Tell the dApp that the transaction was successfull

            // Hide this layout
            viewModel.IsVisible = false;
        }
        catch (Exception ex)
        {
            errorLabel.Text = ex.Message;
            errorLabel.IsVisible = true;
        }

    }

    async void OnRejectClicked(System.Object sender, System.EventArgs e)
    {
        // Maybe send a refuse message 

        // Hide this layout
        var viewModel = DependencyService.Get<TransactionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}

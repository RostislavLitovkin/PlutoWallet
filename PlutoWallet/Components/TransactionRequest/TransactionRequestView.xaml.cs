
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

            // This will need the AssetId update.
            // I was lazy now to fill it in, because it is very rare.
            int _p = 0;

            ChargeType charge;
            if (payload.tip.Length == 34) {
                charge = new ChargeTransactionPayment(HexStringToUint(payload.tip));
                    }
            else
            {
                charge = new ChargeAssetTxPayment(0, 0);
                charge.Decode(Utils.HexToByteArray(payload.tip), ref _p);
            }

            if ((await Model.KeysModel.GetAccount()).IsSome(out var account))
            {
                UnCheckedExtrinsic unCheckedExtrinsic = new UnCheckedExtrinsic(true, account, method, Era.Decode(Utils.HexToByteArray(payload.era)),
                    HexStringToUint(payload.nonce), charge, genesisHash, blockHash);

                byte[] array = unCheckedExtrinsic.GetPayload(runtime).Encode();

                Console.WriteLine("Payload encoded: " + Utils.Bytes2HexString(array));

                Console.WriteLine("Nonce: " + HexStringToUint(payload.nonce));

                var extrinsic = await RequestGenerator.SubmitExtrinsicAsync(
                    true,
                    account,
                    method,
                    Era.Decode(Utils.HexToByteArray(payload.era)),
                    HexStringToUint(payload.nonce),
                    charge,
                    genesisHash,
                    blockHash,
                    runtime
                );

                var signerResult = new SignerResult
                {
                    id = 1,
                    signature = Utils.Bytes2HexString(new byte[1] { 1 }.Concat(extrinsic.Signature).ToArray()).ToLower(),
                };

                await PlutonicationWalletClient.SendPayloadSignatureAsync(signerResult);
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

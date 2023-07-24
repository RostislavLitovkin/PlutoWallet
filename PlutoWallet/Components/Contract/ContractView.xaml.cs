using PlutoWallet.Model;
using System;
using System.Linq;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Newtonsoft.Json;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;
using PlutoWallet.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Rpc;
using PlutoWallet.Components.Extrinsic;

namespace PlutoWallet.Components.Contract;

public partial class ContractView : ContentView
{
	public ContractView()
	{
		InitializeComponent();

		Setup();
	}

	private async Task Setup()
	{
	}

    async void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        try
        {
            var client = Model.AjunaClientModel.Client;

            var (palletIndex, callIndex) = PalletCallModel.GetPalletAndCallIndex(client, "Contracts", "call");




            Method transfer = new Method(palletIndex, "Contracts", callIndex, "call", Utils.HexToByteArray("0x0028f3eab345cc8a8d8ca41cc97342f919c1c54a85883d5fa5ef0281f422643bf40003000032e902000800001012bd51d3"));

            var charge = ChargeTransactionPayment.Default();

            UnCheckedExtrinsic extrinsic = await client.GetExtrinsicParametersAsync(
                transfer,
                KeysModel.GetAccount(),
                charge,
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
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}


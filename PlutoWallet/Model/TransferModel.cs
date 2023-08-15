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

namespace PlutoWallet.Model
{
	public class TransferModel
	{

		public static async Task BalancesTransferAsync(string address, CompactInteger amount)
		{
            // Recognize what type of the address it is and convert it into ss58 one



            // transfer
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(0, accountId);

            var baseComAmount = new BaseCom<U128>();
            baseComAmount.Create(amount);

            var client = Model.AjunaClientModel.Client;

            var (palletIndex, callIndex) = PalletCallModel.GetPalletAndCallIndex(client, "Balances", "transfer");

            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(multiAddress.Encode());
            byteArray.AddRange(baseComAmount.Encode());
            Method transfer = new Method(palletIndex, "Balances", callIndex, "transfer", byteArray.ToArray());

            var charge = ChargeTransactionPayment.Default();

            if ((await KeysModel.GetAccount()).IsSome(out var account))
            {
                UnCheckedExtrinsic extrinsic = await client.GetExtrinsicParametersAsync(
                    transfer,
                    account,
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
            else
            {
                // Verification failed, do something about it
            }
        } 
	}
}


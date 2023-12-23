using System;
using Substrate.NetApi;
using PlutoWallet.Model.Storage;
using Newtonsoft.Json;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using PlutoWallet.Components.Extrinsic;
using PlutoWallet.Constants;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Model
{
    public class PlutoWalletSubstrateClient : SubstrateClientExt
    {
        public PlutoWalletSubstrateClient(Endpoint endpoint, Substrate.NetApi.Model.Extrinsics.ChargeType chargeType) :
                base(endpoint, chargeType)
        {

        }

        /// <summary>
        /// A custom method for submitting extrinsics.
        /// Please prefer using this one.
        /// </summary>
        /// <param name="extrinsic">Extrinsic you want to submit</param>
        /// <param name="token">cancellation token</param>
        /// <returns>subscription ID</returns>
        public async Task<string> SubmitExtrinsicAsync(UnCheckedExtrinsic extrinsic, CancellationToken token)
        {
            var extrinsicStackViewModel = DependencyService.Get<ExtrinsicStatusStackViewModel>();

            extrinsicStackViewModel.Update();

            Action<string, ExtrinsicStatus> callback = (string id, ExtrinsicStatus status) =>
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
                    //extrinsicStackViewModel.Extrinsics[id].Hash = status.InBlock;
                    extrinsicStackViewModel.Update();
                }

                else if (status.Finalized != null)
                {
                    Console.WriteLine("Finalized");
                    extrinsicStackViewModel.Extrinsics[id].Status = ExtrinsicStatusEnum.Success;
                    //extrinsicStackViewModel.Extrinsics[id].Hash = status.Finalized;
                    extrinsicStackViewModel.Update();
                }

                else
                    Console.WriteLine(status.ExtrinsicState);
            };

            string extrinsicId = await this.Author.SubmitAndWatchExtrinsicAsync(callback, Utils.Bytes2HexString(extrinsic.Encode()), token);

            var (palletName, callName) = Model.PalletCallModel.GetPalletAndCallName(this, extrinsic.Method.ModuleIndex, extrinsic.Method.CallIndex);

            extrinsicStackViewModel.Extrinsics.Add(
                extrinsicId,
                new ExtrinsicInfo
                {
                    ExtrinsicId = extrinsicId,
                    Status = ExtrinsicStatusEnum.Pending,
                    Endpoint = this.Endpoint,
                    Hash = new Hash(HashExtension.Blake2(extrinsic.Encode(), 256)),
                    CallName = palletName + "." + callName,
                });

            return extrinsicId;
        }
    }
}


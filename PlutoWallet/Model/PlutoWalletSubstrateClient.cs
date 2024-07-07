using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using PlutoWallet.Components.Extrinsic;
using PlutoWallet.Constants;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.Model.AjunaExt;
using System.Collections.ObjectModel;

namespace PlutoWallet.Model
{
    public class PlutoWalletSubstrateClient : SubstrateClientExt
    {
        public PlutoWalletSubstrateClient(Endpoint endpoint, Uri fastestWebSocket, Substrate.NetApi.Model.Extrinsics.ChargeType chargeType) :
                base(endpoint, fastestWebSocket, chargeType)
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
            Hash extrinsicHash = new Hash(HashExtension.Blake2(extrinsic.Encode(), 256));
            string extrinsicHashString = Utils.Bytes2HexString(extrinsicHash);

            var (palletName, callName) = Model.PalletCallModel.GetPalletAndCallName(this, extrinsic.Method.ModuleIndex, extrinsic.Method.CallIndex);

            var extrinsicStackViewModel = DependencyService.Get<ExtrinsicStatusStackViewModel>();

            extrinsicStackViewModel.Update();

            extrinsicStackViewModel.Extrinsics.Add(
                extrinsicHashString,
                new ExtrinsicInfo
                {
                    ExtrinsicId = extrinsicHashString,
                    Status = ExtrinsicStatusEnum.Submitting,
                    Endpoint = this.Endpoint,
                    Hash = extrinsicHash,
                    CallName = palletName + "." + callName,
                });

            extrinsicStackViewModel.Update();

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
            Action<string, ExtrinsicStatus> callback = async (string id, ExtrinsicStatus status) =>
            {
                if (status.ExtrinsicState == ExtrinsicState.Ready)
                {
                    extrinsicStackViewModel.Extrinsics[extrinsicHashString].Status = ExtrinsicStatusEnum.Pending;
                    extrinsicStackViewModel.Update();
                }
                else if (status.ExtrinsicState == ExtrinsicState.Dropped)
                {
                    extrinsicStackViewModel.Extrinsics[extrinsicHashString].Status = ExtrinsicStatusEnum.Dropped;
                    extrinsicStackViewModel.Update();
                }

                else if (status.ExtrinsicState == ExtrinsicState.InBlock)
                {
                    var events = await EventsModel.GetExtrinsicEventsAsync(
                     this,
                     this.Endpoint.Key,
                     status.Hash,
                     extrinsicHash.Encode()
                     );
                    extrinsicStackViewModel.Extrinsics[extrinsicHashString].EventsListViewModel.ExtrinsicEvents = new ObservableCollection<ExtrinsicEvent>(events);

                    var lastEvent = events.Last();

                    if (lastEvent.PalletName == "System" && lastEvent.EventName == "ExtrinsicSuccess")
                    {
                        extrinsicStackViewModel.Extrinsics[extrinsicHashString].Status = ExtrinsicStatusEnum.InBlockSuccess;
                    }
                    else if (lastEvent.PalletName == "System" && lastEvent.EventName == "ExtrinsicFailed")
                    {
                        extrinsicStackViewModel.Extrinsics[extrinsicHashString].Status = ExtrinsicStatusEnum.InBlockFailed;
                    }
                    else
                    {
                        extrinsicStackViewModel.Extrinsics[extrinsicHashString].Status = ExtrinsicStatusEnum.Unknown;
                    }

                    extrinsicStackViewModel.Update();
                }

                else if (status.ExtrinsicState == ExtrinsicState.Finalized)
                {

                    var lastEvent = extrinsicStackViewModel.Extrinsics[extrinsicHashString].EventsListViewModel.ExtrinsicEvents.Last();

                    if (lastEvent.PalletName == "System" && lastEvent.EventName == "ExtrinsicSuccess")
                    {
                        extrinsicStackViewModel.Extrinsics[extrinsicHashString].Status = ExtrinsicStatusEnum.FinalizedSuccess;
                    }
                    else if (lastEvent.PalletName == "System" && lastEvent.EventName == "ExtrinsicFailed")
                    {
                        extrinsicStackViewModel.Extrinsics[extrinsicHashString].Status = ExtrinsicStatusEnum.FinalizedFailed;
                    }
                    else
                    {
                        extrinsicStackViewModel.Extrinsics[extrinsicHashString].Status = ExtrinsicStatusEnum.Unknown;
                    }

                    extrinsicStackViewModel.Update();
                }

                else
                    Console.WriteLine(status.ExtrinsicState);
            };
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates

            string extrinsicId = await this.Author.SubmitAndWatchExtrinsicAsync(callback, Utils.Bytes2HexString(extrinsic.Encode()).ToLower(), token);

            return extrinsicId;
        }
    }
}


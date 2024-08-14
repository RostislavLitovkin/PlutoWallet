using Substrate.NetApi;
using Newtonsoft.Json;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Extrinsics;
using PlutoWallet.Constants;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;

namespace PlutoWallet.Model.AjunaExt
{
	public class SubstrateClientExt
	{
        private static readonly HttpClient _httpClient = new HttpClient();

        public ChargeType DefaultCharge;
        public Endpoint Endpoint { get; set; }
        public Metadata CustomMetadata { get; set; }
        public SubstrateClient SubstrateClient { get; set; }

        private TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
        public async Task<bool> IsConnectedAsync()
        {
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
            var completedTask = await Task.WhenAny(taskCompletionSource.Task, timeoutTask);

            if (completedTask == taskCompletionSource.Task)
            {
                // Task completed within timeout.
                return await taskCompletionSource.Task;
            }
            else
            {
                // Timeout occurred.
                return false;
            }
        }

        public SubstrateClientExt(Endpoint endpoint, Uri fastestWebSocket, Substrate.NetApi.Model.Extrinsics.ChargeType chargeType) 
        {
            Endpoint = endpoint;

            SubstrateClient = GetSubstrateClient(endpoint.Key, fastestWebSocket);
        }

        /// <summary>
        /// Used only for Testing
        /// </summary>
        public SubstrateClientExt(EndpointEnum mockKey, Endpoint endpoint, Uri fastestWebSocket, Substrate.NetApi.Model.Extrinsics.ChargeType chargeType)
        {
            Endpoint = endpoint;

            SubstrateClient = GetSubstrateClient(mockKey, fastestWebSocket);
        }

        /// <summary>
        /// Appart from connecting to the endpoint, this method also loads the metadata
        /// </summary>
        /// <returns>True if connected successfully, False otherwise</returns>
        public virtual async Task<bool> ConnectAndLoadMetadataAsync()
        {
            try
            {
                await SubstrateClient.ConnectAsync();

                CustomMetadata = JsonConvert.DeserializeObject<Metadata>(SubstrateClient.MetaData.Serialize());

                foreach (SignedExtension signedExtension in CustomMetadata.NodeMetadata.Extrinsic.SignedExtensions)
                {
                    if (signedExtension.SignedIdentifier == "ChargeTransactionPayment")
                    {
                        DefaultCharge = ChargeTransactionPayment.Default();
                    }

                    if (signedExtension.SignedIdentifier == "ChargeAssetTxPayment")
                    {
                        DefaultCharge = ChargeAssetTxPayment.Default();
                    }
                }

                taskCompletionSource.SetResult(SubstrateClient.IsConnected);

                return SubstrateClient.IsConnected;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                taskCompletionSource.SetResult(false);

                return false;
            }
        }

        public virtual async Task<string> SubmitExtrinsicAsync(Method method, Account account, Action<string, ExtrinsicStatus> callback, uint lifeTime = 64, CancellationToken token = default)
        {

            ///
            /// This part is temporary fix before the next Substrate.Net.Api version, that would fix the code gen and sign metadata checks
            ///
            #region Temp
            var unCheckedExtrinsic = await GetTempUnCheckedExtrinsicAsync(method, account, lifeTime, token);
            #endregion


            string extrinsicId = await this.SubstrateClient.Author.SubmitAndWatchExtrinsicAsync(callback, Utils.Bytes2HexString(unCheckedExtrinsic.Encode()).ToLower(), token);

            return extrinsicId;
        }

        public async Task<TempUnCheckedExtrinsic> GetTempUnCheckedExtrinsicAsync(Method method, Account account, uint lifeTime, CancellationToken token)
        {
            bool signed = true;

            ///
            /// This part is temporary fix before the next Substrate.Net.Api version, that would fix the code gen and sign metadata checks
            ///
            #region Temp
            uint nonce = await SubstrateClient.System.AccountNextIndexAsync(account.Value, token);

            Hash startEra = await SubstrateClient.Chain.GetFinalizedHeadAsync(token);
            Era era = Era.Create(lifeTime, (await SubstrateClient.Chain.GetHeaderAsync(startEra, token)).Number.Value);

            TempUnCheckedExtrinsic uncheckedExtrinsic = new TempUnCheckedExtrinsic(signed, account, method, era, nonce, DefaultCharge, SubstrateClient.GenesisHash, startEra);

            TempPayload payload = uncheckedExtrinsic.GetPayload(SubstrateClient.RuntimeVersion);
            uncheckedExtrinsic.AddPayloadSignature(await account.SignAsync(payload.Encode()));
            #endregion

            return uncheckedExtrinsic;
        }

        private SubstrateClient GetSubstrateClient(EndpointEnum endpointKey, Uri websocket)
        {
            return endpointKey switch
            {
                EndpointEnum.Polkadot => new Polkadot.NetApi.Generated.SubstrateClientExt(websocket, ChargeTransactionPayment.Default()),
                EndpointEnum.PolkadotAssetHub => new PolkadotAssetHub.NetApi.Generated.SubstrateClientExt(websocket, ChargeTransactionPayment.Default()),
                EndpointEnum.Hydration => new Hydration.NetApi.Generated.SubstrateClientExt(websocket, ChargeTransactionPayment.Default()),
                EndpointEnum.Bifrost => new Bifrost.NetApi.Generated.SubstrateClientExt(websocket, ChargeTransactionPayment.Default()),
                EndpointEnum.Opal => new Opal.NetApi.Generated.SubstrateClientExt(websocket, ChargeTransactionPayment.Default()),
                EndpointEnum.Bajun => new Bajun.NetApi.Generated.SubstrateClientExt(websocket, ChargeTransactionPayment.Default()),
                EndpointEnum.PolkadotPeople => new PolkadotPeople.NetApi.Generated.SubstrateClientExt(websocket, ChargeTransactionPayment.Default()),

                _ => new SubstrateClient(websocket, ChargeTransactionPayment.Default()),
            };
        }
    }
}


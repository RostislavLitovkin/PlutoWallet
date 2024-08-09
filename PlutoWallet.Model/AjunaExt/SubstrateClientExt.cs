using Substrate.NetApi;
using Newtonsoft.Json;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Extrinsics;
using PlutoWallet.Constants;

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
            catch
            {
                taskCompletionSource.SetResult(false);

                return false;
            }
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


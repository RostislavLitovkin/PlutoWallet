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

        public SubstrateClientExt(Endpoint endpoint, Uri fastestWebSocket, Substrate.NetApi.Model.Extrinsics.ChargeType chargeType) 
        {
            Endpoint = endpoint;

            SubstrateClient = GetSubstrateClient(endpoint.Key, fastestWebSocket);
        }

        public async Task ConnectAndLoadMetadataAsync()
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

                _ => new SubstrateClient(websocket, ChargeTransactionPayment.Default()),
            };
        }
    }
}


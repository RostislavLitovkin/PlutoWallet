
using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;
using AssetKey = (PlutoWallet.Constants.EndpointEnum, PlutoWallet.Types.AssetPallet, System.Numerics.BigInteger);
using AssetMetadata = PlutoWallet.Types.AssetMetadata;

namespace PlutoWallet.Model
{
    public class AssetsMetadataModel
    {
        public static Dictionary<AssetKey, AssetMetadata> AssetsMetadataDict = new System.Collections.Generic.Dictionary<AssetKey, AssetMetadata>();

        public static async Task<AssetMetadata> GetAssetMetadataAsync(SubstrateClientExt client, AssetPallet pallet, BigInteger id, CancellationToken token)
        {
            if (AssetsMetadataDict.ContainsKey((client.Endpoint.Key, pallet, id)))
            {
                return AssetsMetadataDict[(client.Endpoint.Key, pallet, id)];
            }

            if (AssetsModel.AssetsDict.ContainsKey((client.Endpoint.Key, pallet, id)))
            {
                return AssetsModel.AssetsDict[(client.Endpoint.Key, pallet, id)];
            }

            return client.Endpoint.Key switch
            {
                EndpointEnum.Hydration => await GetHydrationAssetMetadataAsync((Hydration.NetApi.Generated.SubstrateClientExt)client.SubstrateClient, client.Endpoint, pallet, id, token),
                EndpointEnum.PolkadotAssetHub => await GetPolkadotAssetHubAssetMetadataAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client.SubstrateClient, client.Endpoint, pallet, id, token),       

                // Add support for more chains

                _ => throw new NotImplementedException()
            };
        }

        public static async Task<AssetMetadata> GetHydrationAssetMetadataAsync(Hydration.NetApi.Generated.SubstrateClientExt client, Endpoint endpoint, AssetPallet pallet, BigInteger assetId, CancellationToken token)
        {
            var meta = await client.AssetRegistryStorage.Assets((U32)(uint)assetId, null, token);

            var symbol = Model.ToStringModel.VecU8ToString(meta.Symbol.Value.Value);
            double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(symbol);

            AssetsMetadataDict[(endpoint.Key, pallet, assetId)] = new AssetMetadata
            {
                Symbol = symbol,
                ChainIcon = endpoint.Icon,
                DarkChainIcon = endpoint.DarkIcon,
                Endpoint = endpoint,
                Pallet = AssetPallet.Tokens,
                AssetId = assetId,
                Decimals = meta.Decimals.Value,
            };

            return AssetsMetadataDict[(endpoint.Key, pallet, assetId)];
        }

        public static async Task<AssetMetadata> GetPolkadotAssetHubAssetMetadataAsync(PolkadotAssetHub.NetApi.Generated.SubstrateClientExt client, Endpoint endpoint, AssetPallet pallet, BigInteger assetId, CancellationToken token)
        {
            var meta = await client.AssetsStorage.Metadata((U32)(uint)assetId, null, token);

            var symbol = Model.ToStringModel.VecU8ToString(meta.Symbol.Value.Value);
            double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(symbol);

            AssetsMetadataDict[(endpoint.Key, pallet, assetId)] = new AssetMetadata
            {
                Symbol = symbol,
                ChainIcon = endpoint.Icon,
                DarkChainIcon = endpoint.DarkIcon,
                Endpoint = endpoint,
                Pallet = AssetPallet.Tokens,
                AssetId = assetId,
                Decimals = meta.Decimals.Value,
            };

            return AssetsMetadataDict[(endpoint.Key, pallet, assetId)];
        }
    }
}

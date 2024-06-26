using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using PlutoWallet.Model.AjunaExt;
using Hydration.NetApi.Generated.Model.pallet_omnipool.types;
using Hydration.NetApi.Generated.Model.orml_tokens;

namespace PlutoWallet.Model.HydraDX
{
    public class Sdk
    {
        const int SYSTEM_ASSET_ID = 0;

        public static Dictionary<string, HydraDXTokenInfo> Assets = new Dictionary<string, HydraDXTokenInfo>();

        public static async Task<Dictionary<string, HydraDXTokenInfo>> GetAssets(SubstrateClientExt client, CancellationToken token)
        {
            if (client is null)
            {
                return new Dictionary<string, HydraDXTokenInfo>();
            }

            var omnipoolAccount = new AccountId32();
            omnipoolAccount.Create(Utils.GetPublicKeyFrom(Constants.HydraDX.OMNIPOOL_ADDRESS));

            var omnipoolAssetsKeyBytes = RequestGenerator.GetStorageKeyBytesHash("Omnipool", "Assets");

            string omnipoolAssetsKeyBytesString = Utils.Bytes2HexString(omnipoolAssetsKeyBytes);
            string tokenAccountsKeyBytesString = Utils.Bytes2HexString(RequestGenerator.GetStorageKeyBytesHash("Tokens", "Accounts"));
            string assetMetadataKeyBytesString = Utils.Bytes2HexString(RequestGenerator.GetStorageKeyBytesHash("AssetRegistry", "Assets"));

            byte[] prefix = omnipoolAssetsKeyBytes;

            byte[] startKey = null;

            var storageKeys = (await client.State.GetKeysPagedAsync(prefix, 1000, startKey, string.Empty, token))
               .Select(p => p.ToString().ToLower().Replace(Utils.Bytes2HexString(prefix).ToLower(), ""));

            Dictionary<string, HydraDXTokenInfo> result = new Dictionary<string, HydraDXTokenInfo>();

            if (storageKeys == null || !storageKeys.Any())
            {
                return result;
            }

            var omnipoolAssetsKeys = storageKeys.Select(p => Utils.HexToByteArray(omnipoolAssetsKeyBytesString + p.ToString())).ToList();
            
            var tokenAccountsKeys = storageKeys.Select(p => Utils.HexToByteArray(tokenAccountsKeyBytesString +
                "12649b1d88771b22c15810b80fb0a1a96d6f646c6f6d6e69706f6f6c0000000000000000000000000000000000000000"
                + Utils.Bytes2HexString(HashExtension.Hash(
                    Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat,
                    Model.HashModel.GetU32FromBlake2_128Concat(p.ToString()).Bytes
                 ), Utils.HexStringFormat.Pure))).ToList();

            var assetMetadataKeys = storageKeys.Select(p => Utils.HexToByteArray(assetMetadataKeyBytesString
                + Utils.Bytes2HexString(HashExtension.Hash(
                    Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat,
                    Model.HashModel.GetU32FromBlake2_128Concat(p.ToString()).Bytes
                 ), Utils.HexStringFormat.Pure))).ToList();

            var omnipoolAssetsStorageChangeSets = (await client.State.GetQueryStorageAtAsync(omnipoolAssetsKeys, string.Empty, token)).ElementAt(0).Changes;
            var tokenAccountsStorageChangeSets = (await client.State.GetQueryStorageAtAsync(tokenAccountsKeys, string.Empty, token)).ElementAt(0).Changes;
            var assetMetadataStorageChangeSets = (await client.State.GetQueryStorageAtAsync(assetMetadataKeys, string.Empty, token)).ElementAt(0).Changes;

            if (omnipoolAssetsStorageChangeSets != null)
            {
                for(int i = 0; i < omnipoolAssetsStorageChangeSets.Length; i++)
                {
                    AssetState asset = new AssetState();
                    asset.Create(omnipoolAssetsStorageChangeSets[i][1]);

                    U32 assetId = Model.HashModel.GetU32FromBlake2_128Concat(omnipoolAssetsStorageChangeSets[i][0].Substring(66));

                    if (assetId.Value != SYSTEM_ASSET_ID)
                    {
                        AccountData omnipoolTokens = new AccountData();
                        omnipoolTokens.Create(tokenAccountsStorageChangeSets[i][1]);

                        var assetMetadata = new Hydration.NetApi.Generated.Model.pallet_asset_registry.types.AssetDetails();

                        assetMetadata.Create(assetMetadataStorageChangeSets[i][1]);

                        string symbol = Model.ToStringModel.VecU8ToString(assetMetadata.Symbol.Value.Value);

                        if (result.ContainsKey(symbol))
                        {
                            continue;
                        }

                        double poolBalance = (double)(omnipoolTokens.Free.Value - omnipoolTokens.Frozen.Value) / Math.Pow(10, assetMetadata.Decimals.Value);
                        double hubReserveBalance = (double)(asset.HubReserve.Value) / Math.Pow(10, 12);

                        result.Add(symbol, new HydraDXTokenInfo
                        {
                            Symbol = symbol,
                            PoolBalance = poolBalance,
                            HubReserve = hubReserveBalance,
                            Decimals = assetMetadata.Decimals.Value,
                        });

                    }
                }
            }

            Assets = result;

            return result;
        }

        public static double GetSpotPrice(string tokenSymbol)
        {
            if (!Assets.ContainsKey(tokenSymbol)) {
                return 0;
            }

            if (!Assets.ContainsKey(Constants.HydraDX.STABLE_TOKEN))
            {
                return 0;
            }

            HydraDXTokenInfo token = Assets[tokenSymbol];
            HydraDXTokenInfo usdToken = Assets[Constants.HydraDX.STABLE_TOKEN];
            
            double price_a = token.HubReserve / token.PoolBalance;
            double price_b = usdToken.PoolBalance / usdToken.HubReserve;

            double result = price_a * price_b;

            return result;
        }
    }

    public class HydraDXTokenInfo
    {
        public double PoolBalance { get; set; }
        public double HubReserve { get; set; }
        public string Symbol { get; set; }
        public int Decimals { get; set; }
    }
}


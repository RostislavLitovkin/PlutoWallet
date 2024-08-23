
using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Net;
using System.Numerics;
using AssetKey = (PlutoWallet.Constants.EndpointEnum, PlutoWallet.Types.AssetPallet, System.Numerics.BigInteger);

namespace PlutoWallet.Model
{
    public class TransactionAnalyzerModel
    {
        /// <summary>
        /// Analyze the events and return the currency changes for each address
        /// </summary>
        /// <returns></returns>
        public static async Task<Dictionary<string, Dictionary<AssetKey, Asset>>> AnalyzeEventsAsync(SubstrateClientExt client, IEnumerable<ExtrinsicEvent> events, Endpoint endpoint, CancellationToken token)
        {
            var result = new Dictionary<string, Dictionary<AssetKey, Asset>>();

            foreach (var e in events)
            {
                IEnumerable<(string, AssetKey, BigInteger)> evaluated = e switch
                {
                    /// Balances
                    ExtrinsicEvent { PalletName: "Balances", EventName: "Transfer" } => [
                        // From negative
                        (e.Parameters[0].Value, (endpoint.Key, AssetPallet.Native, 0), -BigInteger.Parse(e.Parameters[2].Value)),
                        // To positive
                        (e.Parameters[1].Value, (endpoint.Key, AssetPallet.Native, 0), BigInteger.Parse(e.Parameters[2].Value))
                    ],
                    ExtrinsicEvent { PalletName: "Balances", EventName: "Deposit" } => [(e.Parameters[0].Value, (endpoint.Key, AssetPallet.Native, 0), BigInteger.Parse(e.Parameters[1].Value))],
                    ExtrinsicEvent { PalletName: "Balances", EventName: "Withdraw" } => [(e.Parameters[0].Value, (endpoint.Key, AssetPallet.Native, 0), -BigInteger.Parse(e.Parameters[1].Value))],
                    ExtrinsicEvent { PalletName: "Balances", EventName: "Minted" } => [(e.Parameters[0].Value, (endpoint.Key, AssetPallet.Native, 0), BigInteger.Parse(e.Parameters[1].Value))],
                    ExtrinsicEvent { PalletName: "Balances", EventName: "Burned" } => [(e.Parameters[0].Value, (endpoint.Key, AssetPallet.Native, 0), -BigInteger.Parse(e.Parameters[1].Value))],

                    /// Tokens
                    ExtrinsicEvent { PalletName: "Tokens", EventName: "Transfer" } => [
                        // From negative
                        (e.Parameters[1].Value, (endpoint.Key, AssetPallet.Tokens, BigInteger.Parse(e.Parameters[0].Value)), -BigInteger.Parse(e.Parameters[3].Value)),
                        // To positive
                        (e.Parameters[2].Value, (endpoint.Key, AssetPallet.Tokens, BigInteger.Parse(e.Parameters[0].Value)), BigInteger.Parse(e.Parameters[3].Value))
                    ],
                    ExtrinsicEvent { PalletName: "Tokens", EventName: "Deposited" } => [(e.Parameters[1].Value, (endpoint.Key, AssetPallet.Tokens, BigInteger.Parse(e.Parameters[0].Value)), BigInteger.Parse(e.Parameters[2].Value))],
                    ExtrinsicEvent { PalletName: "Tokens", EventName: "Withdrawn" } => [(e.Parameters[1].Value, (endpoint.Key, AssetPallet.Tokens, BigInteger.Parse(e.Parameters[0].Value)), -BigInteger.Parse(e.Parameters[2].Value))],

                    /// Assets
                    ExtrinsicEvent { PalletName: "Assets", EventName: "Issued" } => [(e.Parameters[1].Value, (endpoint.Key, AssetPallet.Assets, BigInteger.Parse(e.Parameters[0].Value)), BigInteger.Parse(e.Parameters[2].Value))],
                    ExtrinsicEvent { PalletName: "Assets", EventName: "Burned" } => [(e.Parameters[1].Value, (endpoint.Key, AssetPallet.Assets, BigInteger.Parse(e.Parameters[0].Value)), BigInteger.Parse(e.Parameters[2].Value))],

                    /// Fees
                    ExtrinsicEvent { PalletName: "TransactionPayment", EventName: "TransactionFeePaid" } => [("fee", (endpoint.Key, AssetPallet.Native, 0), -BigInteger.Parse(e.Parameters[1].Value) - BigInteger.Parse(e.Parameters[2].Value))],
                    ExtrinsicEvent { PalletName: "XcmPallet", EventName: "FeesPaid" } => EvaluateXcmPalletFeesPaid(e, endpoint),

                    // Handle more events ...
                    _ => []
                };

                foreach (var (address, key, amount) in evaluated)
                {
                    if (!result.ContainsKey(address))
                    {
                        result[address] = new Dictionary<AssetKey, Asset>();
                    }

                    if (!result[address].ContainsKey(key))
                    {
                        result[address][key] = key.Item2 switch
                        {
                            AssetPallet.Native => new Asset
                            {
                                Amount = 0,
                                Pallet = key.Item2,
                                Symbol = endpoint.Unit,
                                ChainIcon = endpoint.Icon,
                                DarkChainIcon = endpoint.DarkIcon,
                                AssetId = key.Item3,
                                Endpoint = endpoint,
                                Decimals = endpoint.Decimals
                            },
                            _ => (await AssetsMetadataModel.GetAssetMetadataAsync(client, key.Item2, key.Item3, token)).ToAsset()
                        };
                    }

                    result[address][key].Amount += (double)amount / Math.Pow(10, result[address][key].Decimals);
                }
            }

            /// Remove emptry values
            foreach (var address in result.Keys)
            {
                foreach(var assetKey in result[address].Keys)
                {
                    if (result[address][assetKey].Amount == 0)
                    {
                        result[address].Remove(assetKey);
                    }
                }

                if (result[address].Keys.Count() == 0)
                {
                    result.Remove(address);
                }
            }

            return result;
        }

        private static IEnumerable<(string, AssetKey, BigInteger)> EvaluateXcmPalletFeesPaid(ExtrinsicEvent e, Endpoint endpoint)
        {
            var feeAssets = new Polkadot.NetApi.Generated.Model.staging_xcm.v4.asset.Assets();
            int p = 0;
            feeAssets.Decode(e.Parameters[1].EncodedValue, ref p);

            return feeAssets.Value.Value.Select((asset) =>
            {
                var assetKey = XcmModel.GetAssetFromXcmLocation(asset.Id.Value, endpoint);

                if (assetKey == null)
                {
                    // Bad
                    return ("fee", (endpoint.Key, AssetPallet.Native, (BigInteger)0), 0);
                }

                return ("fee", (endpoint.Key, AssetPallet.Native, (BigInteger)0), -(BigInteger)((BaseCom<U128>)asset.Fun.Value2).Value);
            });
        }
    }
}


using PlutoWallet.Constants;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
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
        public static Dictionary<string, Dictionary<AssetKey, Asset>> AnalyzeEvents(IEnumerable<ExtrinsicEvent> events, Endpoint endpoint)
        {
            var result = new Dictionary<string, Dictionary<AssetKey, Asset>>();

            foreach (var e in events)
            {
                IEnumerable<(string, AssetKey, BigInteger)> evaluated = e switch
                {
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

                    // Handle fees
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
                        result[address][key] = new Asset
                        {
                            Amount = 0,
                            Pallet = key.Item2,
                            Symbol = key.Item2 switch
                            {
                                AssetPallet.Native => endpoint.Unit,
                                // Fill later
                                _ => "Unknown"
                            },
                            ChainIcon = endpoint.Icon,
                            DarkChainIcon = endpoint.DarkIcon,
                            AssetId = key.Item3,
                            Endpoint = endpoint,
                        };
                    }

                    var decimals = key.Item2 switch
                    {
                        AssetPallet.Native => endpoint.Decimals,
                        // Fill later
                        _ => 0
                    };

                    result[address][key].Amount += (double)amount / Math.Pow(10, decimals);
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

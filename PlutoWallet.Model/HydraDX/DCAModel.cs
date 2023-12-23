using System;
using Substrate.NetApi.Generated.Model.orml_tokens;
using Substrate.NetApi.Generated.Model.pallet_asset_registry.types;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Substrate.NetApi;
using Substrate.NetApi.Generated.Model.pallet_omnipool.types;
using Substrate.NetApi.Model.Types.Primitive;
using static Substrate.NetApi.Model.Meta.Storage;
using System.Numerics;
using Substrate.NetApi.Generated.Model.pallet_dca.types;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Model.HydraDX
{
	public class DCAModel
	{
        public static async Task<List<DCAPosition>> GetDCAPositions(SubstrateClientExt client, string substrateAddress)
        {
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(substrateAddress));

            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("DCA", "ScheduleOwnership");

            byte[] prefix = keyBytes.Concat(HashExtension.Hash(Hasher.BlakeTwo128Concat, account32.Encode())).ToArray();

            string prefixString = Utils.Bytes2HexString(prefix);

            byte[] startKey = null;

            List<U32> positionIds;

            var keysPaged = await client.State.GetKeysPagedAtAsync(prefix, 1000, startKey, string.Empty, CancellationToken.None);

            if (keysPaged == null || !keysPaged.Any())
            {
                return new List<DCAPosition>();
            }
            else
            {
                positionIds = keysPaged.Select(p => HashModel.GetU32FromTwox_64Concat(p.ToString().Substring(162))).ToList();
            }

            List<DCAPosition> result = new List<DCAPosition>();

            foreach (U32 positionId in positionIds)
            {

                Schedule schedule = await client.DCAStorage.Schedules(positionId, CancellationToken.None);

                if (schedule.Order.Value.ToString() != "Sell")
                {
                    continue;
                }

                U128 amount = await client.DCAStorage.RemainingAmounts(positionId, CancellationToken.None);

                int i = 0;
                var scheduleOrder = new BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U128, Substrate.NetApi.Model.Types.Primitive.U128, Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT9>();
                scheduleOrder.Decode(schedule.Order.Value2.Encode(), ref i);

                AssetMetadata fromAsset = await client.AssetRegistryStorage.AssetMetadataMap((U32)scheduleOrder.Value[0], CancellationToken.None);

                AssetMetadata toAsset = await client.AssetRegistryStorage.AssetMetadataMap((U32)scheduleOrder.Value[1], CancellationToken.None);

                // Get the time left for the DCA order
                BigInteger times = (schedule.TotalAmount.Value - amount.Value) / ((U128)scheduleOrder.Value[2]).Value + 1;

                BigInteger blocks = times * schedule.Period.Value;

                Console.WriteLine("Times: " + times);

                Console.WriteLine("Blocks: " + blocks);

                TimeSpan time = new TimeSpan((long)(blocks) * 120000000 + 3600 * (long)120000000);

                Console.WriteLine("Seconds: " + time.TotalSeconds);

                result.Add(new DCAPosition
                {
                    Amount = (double)amount.Value / Math.Pow(10, fromAsset.Decimals.Value),
                    FromSymbol = Model.ToStringModel.VecU8ToString(fromAsset.Symbol.Value.Value),
                    ToSymbol = Model.ToStringModel.VecU8ToString(toAsset.Symbol.Value.Value),
                    RemainingDays = time.Days,
                    RemainingHours = time.Hours,
                });
            }

            return result;
        }
    }

    public class DCAPosition
    {
        public double Amount { get; set; }
        public string FromSymbol { get; set; }
        public string ToSymbol { get; set; }
        public int RemainingDays { get; set; }
        public int RemainingHours { get; set; }
    }
}


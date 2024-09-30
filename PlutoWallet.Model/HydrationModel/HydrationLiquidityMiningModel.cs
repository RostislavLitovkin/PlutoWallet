using Hydration.NetApi.Generated;
using Hydration.NetApi.Generated.Model.pallet_liquidity_mining.types;
using Hydration.NetApi.Generated.Model.sp_arithmetic.fixed_point;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;
using System;
using PlutoWallet.Model.HydraDX;
using Substrate.NetApi;
using Hydration.NetApi.Generated.Model.pallet_omnipool.types;

namespace PlutoWallet.Model.HydrationModel
{
    public class OmnipoolLiquidityMiningInfo
    {
        public double RewardAmount { get; set; }
        public uint RewardAssetId { get; set; }
        public string RewardSymbol { get; set; }
    }
    public class OmnipoolLiquidityInfoExpanded : OmnipoolLiquidityInfo
    {
        public List<OmnipoolLiquidityMiningInfo> LiquidityMiningInfos { get; set; }
    }

    public static class HydrationLiquidityMiningModel
    {
        public static async Task<List<OmnipoolLiquidityInfoExpanded>> GetOmnipoolLiquidityWithLiquidityMining(SubstrateClientExt substrateClient, string substrateAddress, CancellationToken token = default)
        {
            var liquidityMiningPositionIds = await UniquesModel.GetUniquesInCollection(substrateClient, 2584, substrateAddress, token);

            List<OmnipoolLiquidityInfoExpanded> result = new List<OmnipoolLiquidityInfoExpanded>();

            foreach (U128 liquidityMiningPositionId in liquidityMiningPositionIds)
            {
                var positionId = await substrateClient.OmnipoolLiquidityMiningStorage.OmniPositionId(liquidityMiningPositionId, null, token);

                var liquidityInfo = await HydrationOmnipoolModel.GetOmnipoolLiquidityAtPositionAsync(substrateClient, positionId, token);

                result.Add(new OmnipoolLiquidityInfoExpanded
                {
                    Amount = liquidityInfo.Amount,
                    AssetId = liquidityInfo.AssetId,
                    Symbol = liquidityInfo.Symbol,
                    InitialAmount = liquidityInfo.InitialAmount,
                    LiquidityMiningInfos = await GetLiquidityMiningDeposit(substrateClient, liquidityMiningPositionId, token),
                });
            }

            return result;
        }

        public static async Task<List<OmnipoolLiquidityMiningInfo>> GetLiquidityMiningDeposit(SubstrateClientExt substrateClient, U128 id, CancellationToken token = default)
        {
            var data = await substrateClient.OmnipoolWarehouseLMStorage.Deposit(id, null, token);

            var liquidityMiningData = new List<OmnipoolLiquidityMiningInfo>();

            foreach (YieldFarmEntryT1 farmEntry in data?.YieldFarmEntries.Value.Value ?? [])
            {
                var globalFarmData = await substrateClient.OmnipoolWarehouseLMStorage.GlobalFarm(farmEntry.GlobalFarmId, null, token);

                var yieldFarmData = await substrateClient.OmnipoolWarehouseLMStorage.YieldFarm(
                    new BaseTuple<U32, U32, U32>(data.AmmPoolId, farmEntry.GlobalFarmId, farmEntry.YieldFarmId), null, token
                );

                if(Sdk.AssetsById.Count() == 0) { 
                    await Sdk.GetAssets(substrateClient, token);
                }


                var assetInfo = Sdk.AssetsById[globalFarmData.RewardCurrency.Value];

                var deltaStopped = yieldFarmData.TotalStopped - farmEntry.StoppedAtCreation;

                var periods = yieldFarmData.UpdatedAt - farmEntry.EnteredAt - deltaStopped;

                var loyaltyMultiplier = GetLoyaltyMultiplier(periods, yieldFarmData.LoyaltyCurve);

                var (rewards, unclaimableRwards) = CalculateUserReward(
                    farmEntry.AccumulatedRpvs.ToDecimal(),
                    farmEntry.ValuedShares,
                    farmEntry.AccumulatedClaimedRewards,
                    yieldFarmData.AccumulatedRpvs.ToDecimal(),
                    loyaltyMultiplier
                );

                liquidityMiningData.Add(new OmnipoolLiquidityMiningInfo
                {
                    RewardAmount = (double)(rewards - unclaimableRwards) / Math.Pow(10, assetInfo.Decimals),
                    RewardSymbol = assetInfo.Symbol,
                    RewardAssetId = globalFarmData.RewardCurrency.Value,
                });
            }

            return liquidityMiningData;
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/master/math/src/liquidity_mining/liquidity_mining.rs#L55
        /// </summary>
        /// <returns></returns>
        public static (BigInteger, BigInteger) CalculateUserReward(
            decimal accumulatedRpvs,
            BigInteger valuedShares,
            BigInteger accumulatedClaimedRewards,
            decimal accumulatedRpvsNow,
            decimal loyaltyMultiplier
        )
        {
            var maxRewards = CalculateReward(accumulatedRpvs, accumulatedRpvsNow, valuedShares);

            if (maxRewards == 0)
            {
                return (0, 0);
            }

            var claimableRewards = (BigInteger)(loyaltyMultiplier * (decimal)maxRewards);

            var unclaimableRewards = maxRewards - claimableRewards;

            var user_rewards = claimableRewards - accumulatedClaimedRewards;

            return (user_rewards, unclaimableRewards);
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/master/math/src/liquidity_mining/liquidity_mining.rs#L95
        /// </summary>
        /// <returns></returns>
        public static BigInteger CalculateReward(
            decimal accumulatedRpsStart,
            decimal accumulatedRpsNow,
            BigInteger shares
        )
        {
            return (BigInteger)((accumulatedRpsNow - accumulatedRpsStart) * (decimal)shares);
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/master/pallets/liquidity-mining/src/lib.rs#L1509
        /// </summary>
        /// <returns></returns>
        public static decimal GetLoyaltyMultiplier(
            uint periods,
            Substrate.NetApi.Model.Types.Base.BaseOpt<Hydration.NetApi.Generated.Model.pallet_liquidity_mining.types.LoyaltyCurve> curveOption
            )
        {
            if (!curveOption.OptionFlag)
            {
                return 1;
            }

            var curve = curveOption.Value;

            return CalculateLoyaltyMultiplier(periods, curve.InitialRewardPercentage.ToDecimal(), curve.ScaleCoef);

        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/master/math/src/liquidity_mining/liquidity_mining.rs#L22
        /// </summary>
        /// <returns></returns>
        public static decimal CalculateLoyaltyMultiplier(
            uint period,
            decimal initialRewardPercentage,
            uint scaleCoef
            )
        {
            var num = initialRewardPercentage * scaleCoef + period;
            var denom = scaleCoef + period;

            return num / denom;
        }

        public static decimal ToDecimal(this FixedU128 fixedU128)
        {
            return (decimal)fixedU128.Value.Value / 1_000_000_000_000_000_000;
        }
    }
}

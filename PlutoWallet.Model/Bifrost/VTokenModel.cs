using System;
using System.Numerics;
using Bifrost.NetApi.Generated;
using Bifrost.NetApi.Generated.Model.bifrost_primitives.currency;
using Bifrost.NetApi.Generated.Model.sp_arithmetic.per_things;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Primitive;

namespace PlutoWallet.Model
{
	public class VTokenModel
	{
        private static Dictionary<EnumCurrencyId, U128> totalIssuanceDictionary = new Dictionary<EnumCurrencyId, U128>();
        private static Dictionary<EnumCurrencyId, U128> tokenPoolDictionary = new Dictionary<EnumCurrencyId, U128>();

        /// <summary>
		/// Copy of https://github.com/bifrost-finance/bifrost/blob/7b09365c56ec528955c33ddf414a3322382053f0/pallets/vtoken-minting/src/lib.rs#L1289
		/// </summary>
		/// <param name="vTokenId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
        public static async Task<BigInteger> VDotToDot(SubstrateClientExt client, BigInteger amount, CancellationToken token)
        {
            // VDOT
            var substrateVTokenId = new EnumCurrencyId();
            substrateVTokenId.Create(CurrencyId.VToken2, new U8(0));

            // DOT
            var substrateTokenId = new EnumCurrencyId();
            substrateTokenId.Create(CurrencyId.Token2, new U8(0));

            return await RedeemInner(client, substrateVTokenId, amount, substrateTokenId, token);
        }

        /// <summary>
		/// Copy of https://github.com/bifrost-finance/bifrost/blob/7b09365c56ec528955c33ddf414a3322382053f0/pallets/vtoken-minting/src/lib.rs#L1289
		/// </summary>
		/// <param name="vTokenId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
        public static async Task<BigInteger> RedeemInner(SubstrateClientExt client, EnumCurrencyId inTokenId, BigInteger inAmount, EnumCurrencyId toTokenId, CancellationToken token)
        {
            // 1) Subtract redeem fee
            var fees = await client.VtokenMintingStorage.Fees(null, token);

            inAmount -= (BigInteger)((double)inAmount * (1.0 / ((Permill)fees.Value[1]).Value.Value));

            // 2) Get tokens amount in pool
            U128 toTokensInPool;
            if (tokenPoolDictionary.ContainsKey(toTokenId))
            {
                toTokensInPool = tokenPoolDictionary[toTokenId];
            }
            else
            {
                toTokensInPool = await client.VtokenMintingStorage.TokenPool(toTokenId, null, token);
                tokenPoolDictionary[toTokenId] = toTokensInPool;
            }

            // 3) Get token issuance
            U128 inTokensTotalIssance;
            if (totalIssuanceDictionary.ContainsKey(inTokenId))
            {
                inTokensTotalIssance = totalIssuanceDictionary[inTokenId];
            }
            else
            {
                inTokensTotalIssance = await BifrostTokenTotalIssuance(client, inTokenId, token);
                totalIssuanceDictionary[inTokenId] = inTokensTotalIssance;
            }
            
            return inAmount * toTokensInPool / inTokensTotalIssance;
        }

        /// <summary>
        /// >> TotalIssuance
        ///  The total issuance of a token type.
        /// </summary>
        public static async Task<Substrate.NetApi.Model.Types.Primitive.U128> BifrostTokenTotalIssuance(SubstrateClientExt client, EnumCurrencyId key, CancellationToken token)
        {
            string parameters = RequestGenerator.GetStorage("Tokens", "TotalIssuance", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
            var result = await client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, token);
            return result;
        }
    }
}


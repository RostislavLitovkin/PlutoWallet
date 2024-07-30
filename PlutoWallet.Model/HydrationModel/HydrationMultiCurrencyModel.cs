using Hydration.NetApi.Generated;
using Hydration.NetApi.Generated.Model.sp_core.crypto;
using Substrate.NetApi.Model.Types.Primitive;

namespace PlutoWallet.Model.HydrationModel
{
    public static class HydrationMultiCurrencyModel
    {
        public static async Task<U128> GetFreeBalanceAsync(this SubstrateClientExt client, AccountId32 accountId, U32 assetId, CancellationToken token)
        {
            switch (assetId.Value)
            {
                case 0:
                    var accountInfo = await client.SystemStorage.Account(accountId, null, token);
                    return accountInfo.Data.Free;

                default:
                    var omnipoolTokensKey = new Substrate.NetApi.Model.Types.Base.BaseTuple<AccountId32, Substrate.NetApi.Model.Types.Primitive.U32>();
                    omnipoolTokensKey.Create(accountId, assetId);

                    var omnipoolTokens = await client.TokensStorage.Accounts(omnipoolTokensKey, null, token);

                    return omnipoolTokens.Free;  
            };
        }
    }
}

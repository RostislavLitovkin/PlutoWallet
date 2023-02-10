using System;
using Ajuna.NetApi;
using Ajuna.NetApi.Model.Extrinsics;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Types.AjunaExtTypes;

namespace PlutoWallet.Model
{
	public class TransferModel
	{

		public static async Task BalancesTransferAsync(string address, CompactInteger amount)
		{
            // Recognize what type of the address it is and convert it into ss58 one



            // transfer
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(0, accountId);

            var baseComAmount = new BaseCom<U128>();
            baseComAmount.Create(amount);

            Method transfer = BalancesCalls.Transfer(multiAddress, baseComAmount);


            var client = new SubstrateClient(new Uri("ws://127.0.0.1:9944"), ChargeTransactionPayment.Default());
            await client.ConnectAsync();

            await client.Author.SubmitExtrinsicAsync(transfer, KeysModel.GetAccount(), ChargeTransactionPayment.Default(), 64);
        } 
	}
}


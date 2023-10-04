using System;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Substrate.NetApi;
using Substrate.NetApi.Generated.Model.pallet_identity.types;
using PlutoWallet.Constants;

namespace PlutoWallet.Model
{
	public class IdentityModel
	{
		public static async Task<OnChainIdentity> GetIdentityForAddress(string address)
		{
			var client = Model.AjunaClientModel.Client;

            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(address));

            Registration registration = await client.IdentityStorage.IdentityOf(account32, CancellationToken.None);

			return new OnChainIdentity
			{
				DisplayName = System.Text.Encoding.UTF8.GetString(registration.Info.Display.Value2.Encode())
			};
		}
	}

	public class OnChainIdentity
	{
		public string DisplayName { get; set; }
		public string FinalJudgement { get; set; }
		public string LegalName { get; set; }
		public Endpoint Endpoint { get; set; }
	}
}


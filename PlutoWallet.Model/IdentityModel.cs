using System;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Substrate.NetApi;
using Substrate.NetApi.Generated.Model.pallet_identity.types;
using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Model
{
	public class IdentityModel
	{
		public static async Task<OnChainIdentity> GetIdentityForAddress(SubstrateClientExt client, string address)
		{
			try
			{
				if (address == null)
				{
					return null;
				}

				var account32 = new AccountId32();
				account32.Create(Utils.GetPublicKeyFrom(address));

				Registration registration = await client.IdentityStorage.IdentityOf(account32, CancellationToken.None);

				if (registration == null)
				{
					return null;
				}

				Judgement finalJudgement = Judgement.Unknown;

				foreach (var thing in registration.Judgements.Value.Value)
				{
					switch (thing.Value[1].ToString())
					{
						case "0":
							finalJudgement = Judgement.Unknown;
							break;
						case "1":
							// fee paid
							break;
						case "2":
							finalJudgement = Judgement.Reasonable;
							break;
						case "3":
							finalJudgement = Judgement.KnownGood;
							break;
						case "4":
							finalJudgement = Judgement.OutOfDate;
							break;
						case "5":
							finalJudgement = Judgement.LowQuality;
							break;
						case "6":
							finalJudgement = Judgement.Erroneous;
							break;
					}
				}

				return new OnChainIdentity
				{
					DisplayName = System.Text.Encoding.UTF8.GetString(registration.Info.Display.Value2.Encode()),
					FinalJudgement = finalJudgement,
				};
			}
			catch
			{
				return null;
			}
		}
	}

	public class OnChainIdentity
	{
		public string DisplayName { get; set; }
		public Judgement FinalJudgement { get; set; }
		public string LegalName { get; set; }
		public Endpoint Endpoint { get; set; }
	}

	public enum Judgement
	{
		Unknown,
		Reasonable,
        KnownGood,
        OutOfDate,
        LowQuality,
        Erroneous,
    }
}


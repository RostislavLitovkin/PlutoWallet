using PolkadotPeople.NetApi.Generated.Model.sp_core.crypto;
using PlutoWallet.Constants;
using Substrate.NetApi;

namespace PlutoWallet.Model
{
	public class IdentityModel
	{
		public static async Task<OnChainIdentity?> GetIdentityForAddressAsync(PolkadotPeople.NetApi.Generated.SubstrateClientExt client, string address)
		{
			try
			{
				if (address == null)
				{
					return null;
				}

				var account32 = new AccountId32();
				account32.Create(Utils.GetPublicKeyFrom(address));

				var identity = await client.IdentityStorage.IdentityOf(account32, null, CancellationToken.None);

				if (identity == null)
				{
					return null;
				}

				Judgement finalJudgement = Judgement.Unknown;

				var registration = ((PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.Registration)identity.Value[0]);

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
						default:
							finalJudgement = Judgement.Unknown;
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


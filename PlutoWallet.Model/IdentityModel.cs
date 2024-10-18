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

                foreach (Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.EnumJudgement> thing in registration.Judgements.Value.Value)
				{
					switch (((PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.EnumJudgement)thing.Value[1]).Value)
					{
						case PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.Judgement.Unknown:
							finalJudgement = Judgement.Unknown;
							break;
						case PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.Judgement.FeePaid:
							// fee paid
							break;
						case PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.Judgement.Reasonable:
							finalJudgement = Judgement.Reasonable;
							break;
						case PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.Judgement.KnownGood:
							finalJudgement = Judgement.KnownGood;
							break;
						case PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.Judgement.OutOfDate:
							finalJudgement = Judgement.OutOfDate;
							break;
						case PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.Judgement.LowQuality:
							finalJudgement = Judgement.LowQuality;
							break;
						case PolkadotPeople.NetApi.Generated.Model.pallet_identity.types.Judgement.Erroneous:
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
		public required string DisplayName { get; set; }
		public Judgement FinalJudgement { get; set; }
		//public required string LegalName { get; set; }
		//public Endpoint Endpoint { get; set; }
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


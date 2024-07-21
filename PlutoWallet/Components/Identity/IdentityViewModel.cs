using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;

namespace PlutoWallet.Components.Identity
{
	public partial class IdentityViewModel : ObservableObject
	{
		[ObservableProperty]
		private string name;

        [ObservableProperty]
        private string verificationIcon;

        [ObservableProperty]
        private bool verificationIconIsVisible;

        public IdentityViewModel()
		{
            verificationIconIsVisible = false;
		}

        public async Task GetIdentity(Polkadot.NetApi.Generated.SubstrateClientExt client)
        {
            if (client is null)
            {
                Name = "Failed";

                return;
            }

            Name = "Loading";
            VerificationIconIsVisible = false;

            var identity = await IdentityModel.GetIdentityForAddress(client, KeysModel.GetSubstrateKey());

            if (identity == null)
            {
                Name = "None";
                return;
            }

            Name = identity.DisplayName;
            VerificationIconIsVisible = true;

            switch (identity.FinalJudgement)
            {
                case Judgement.Unknown:
                    if (Application.Current.RequestedTheme == AppTheme.Light)
                    {
                        VerificationIcon = "unknownblack.png";
                    }
                    else
                    {
                        VerificationIcon = "unknownwhite.png";
                    }
                    break;
                case Judgement.LowQuality:
                    if (Application.Current.RequestedTheme == AppTheme.Light)
                    {
                        VerificationIcon = "unknownblack.png";
                    }
                    else
                    {
                        VerificationIcon = "unknownwhite.png";
                    }
                    break;
                case Judgement.OutOfDate:
                    if (Application.Current.RequestedTheme == AppTheme.Light)
                    {
                        VerificationIcon = "unknownblack.png";
                    }
                    else
                    {
                        VerificationIcon = "unknownwhite.png";
                    }
                    break;
                case Judgement.Reasonable:
                    VerificationIcon = "greentick.png";
                    break;
                case Judgement.KnownGood:
                    VerificationIcon = "greentick.png";
                    break;
                case Judgement.Erroneous:
                    VerificationIcon = "redallert.png";
                    break;
            }
        }
	}
}


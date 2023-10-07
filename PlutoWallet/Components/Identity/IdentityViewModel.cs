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


        public IdentityViewModel()
		{
		}

        public async Task GetIdentity()
        {
            Name = "Loading";
            var identity = await Model.IdentityModel.GetIdentityForAddress(Model.KeysModel.GetSubstrateKey());

            if (identity == null)
            {
                Name = "None";
                return;
            }

            Name = identity.DisplayName;

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


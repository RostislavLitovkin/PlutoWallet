using System;
namespace PlutoWallet.Components.Extrinsic
{
	public enum ExtrinsicStatusEnum
	{
        NothingUpdateUIBug,
		Submitting,
		Error,
        Pending,
        InBlockSuccess,
        InBlockFailed,
        FinalizedSuccess,
        FinalizedFailed,
        Dropped,
        Unknown,
	}
}


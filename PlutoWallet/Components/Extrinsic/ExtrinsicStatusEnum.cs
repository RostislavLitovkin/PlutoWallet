using System;
namespace PlutoWallet.Components.Extrinsic
{
	public enum ExtrinsicStatusEnum
	{
		Submitting,
		Error,
        Pending,
        InBlock,
		Finalized,
		Failed,
	}
}


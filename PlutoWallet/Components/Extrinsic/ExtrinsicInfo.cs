using System;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.Extrinsic
{
	public class ExtrinsicInfo
	{
		public string ExtrinsicId { get; set; }
		public ExtrinsicStatusEnum Status { get; set; }
        public Endpoint Endpoint { get; set; }
		public Hash Hash { get; set; }
		public string CallName { get; set; }
    }
}


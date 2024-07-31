using System;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.Constants;
using PlutoWallet.Components.Events;
using System.Numerics;

namespace PlutoWallet.Components.Extrinsic
{
	public class ExtrinsicInfo
	{
		public string ExtrinsicId { get; set; }
		public ExtrinsicStatusEnum Status { get; set; }
		public EventsListViewModel EventsListViewModel { get; set; } = new EventsListViewModel();
        public Endpoint Endpoint { get; set; }
		public Hash Hash { get; set; }
		public string CallName { get; set; }
        public BigInteger BlockNumber { get; set; }
        public uint? ExtrinsicIndex { get; set; }
    }
}


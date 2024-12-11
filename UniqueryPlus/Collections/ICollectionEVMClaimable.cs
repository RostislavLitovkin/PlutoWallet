using Substrate.NetApi.Model.Extrinsics;
using UniqueryPlus.EVM;

namespace UniqueryPlus.Collections
{
    internal interface ICollectionEVMClaimable
    {
        public Task<EventConfig?> GetEventInfoAsync(CancellationToken token);
        public Method Claim(string sender);
    }
}

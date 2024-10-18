using Substrate.NetApi.Model.Extrinsics;
namespace UniqueryPlus.Nfts
{
    public interface INftBurnable
    {
        public bool IsBurnable { get; set; }
        public Method Burn();
    }
}

namespace UniqueryPlus.Nfts
{
    public interface INftBaseNestable : INftBase, INftNestable;
    public interface INftNestable
    {
        Task<IEnumerable<NestedNftWrapper<INftBaseNestable>>> GetNestedNftsAsync(CancellationToken token);
    }

    public record NestedNftWrapper<NftType> where NftType : INftBase, INftBaseNestable 
    { 
        public required NftType NftBase { get; init; }
        public required uint Depth { get; init; }
    }
}

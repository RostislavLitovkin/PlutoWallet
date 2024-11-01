namespace UniqueryPlus.Nfts
{
    public interface INftNestable<NftType> where NftType : INftBase, INftNestable<NftType>
    {
        Task<IEnumerable<NestedNftWrapper<NftType>>> GetNestedNftsAsync(CancellationToken token);
    }

    public record NestedNftWrapper<NftType> where NftType : INftBase, INftNestable<NftType>
    {
        public required NftType NftBase { get; init; }
        public required uint Depth { get; init; }
    }
}

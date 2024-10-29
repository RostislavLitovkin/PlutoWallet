
using System.Numerics;

namespace UniqueryPlus.Nfts
{
    public interface INftMarketPrice
    {
        Task<BigInteger?> GetMarketPriceAsync(CancellationToken token);
    }
}

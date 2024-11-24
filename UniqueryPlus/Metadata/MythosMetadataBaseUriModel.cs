using System.Numerics;

namespace UniqueryPlus.Metadata
{
    internal static class MythosMetadataBaseUriModel
    {
        /// <summary>
        /// Just a cache for fetching the BaseUri of the collection by its CollectionId
        /// </summary>
        public static Dictionary<BigInteger, string> BaseUriCache = new Dictionary<BigInteger, string>();
    }
}

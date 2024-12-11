namespace UniqueryPlus
{
    public class Constants
    {
        // 0x + Twox64 pallet + Twox64 storage
        public const int BASE_STORAGE_KEY_LENGTH = 66;
        public const int BLAKE2_128HASH_LENGTH = 32;

        public const string UNIQUE_IPFS_ENDPOINT = "https://ipfs.unique.network/ipfs/";
        public const string KODA_IPFS_ENDPOINT = "https://image.w.kodadot.xyz/ipfs/";
        public const string DEFAULT_IPFS_ENDPOINT = "https://ipfs.io/ipfs/";

        public const string UNIQUE_EVM_RPC = "https://rpc.unique.network";
        public const string OPAL_EVM_RPC = "https://rpc-opal.unique.network/";
    }
}

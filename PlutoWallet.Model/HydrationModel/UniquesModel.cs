using PlutoWallet.Model.HydraDX;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hydration.NetApi.Generated.Model.sp_core.crypto;
using static Substrate.NetApi.Model.Meta.Storage;

namespace PlutoWallet.Model.HydrationModel
{
    public class UniquesModel
    {
        public static async Task<List<U128>> GetUniquesInCollection(SubstrateClient substrateClient, uint collectionId, string substrateAddress, CancellationToken token)
        {
            // Get all position keys
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(substrateAddress));

            U128 collectionIdU128 = new U128();
            collectionIdU128.Create(collectionId);

            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("Uniques", "Account");

            byte[] prefix = keyBytes.Concat(HashExtension.Hash(Hasher.BlakeTwo128Concat, account32.Encode()))
                .Concat(HashExtension.Hash(Hasher.BlakeTwo128Concat, collectionIdU128.Encode())).ToArray();

            string prefixString = Utils.Bytes2HexString(prefix);

            byte[]? startKey = null;

            var keysPaged = await substrateClient.State.GetKeysPagedAsync(prefix, 1000, startKey, string.Empty, CancellationToken.None);

            if (keysPaged == null || !keysPaged.Any())
            {
                return new List<U128>();
            }
            else
            {
                return keysPaged.Select(p => HashModel.GetU128FromBlake2_128Concat(p.ToString().Substring(226))).ToList();
            }
        }
    }
}

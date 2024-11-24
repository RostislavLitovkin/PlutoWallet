using SQLite;
using PlutoWallet.Model.Constants;
using UniqueryPlus;
using System.Numerics;
using System.Text.Json;
using PlutoWallet.Constants;
using UniqueryPlus.Collections;
using UniqueryPlus.Metadata;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Model.SQLite
{
    public class SavedNftBase : INftBase
    {
        public NftTypeEnum Type { get; set; }
        public BigInteger CollectionId { get; set; }
        public BigInteger Id { get; set; }
        public string Owner { get; set; }
        public MetadataBase Metadata { get; set; }
        public async Task<ICollectionBase> GetCollectionAsync(CancellationToken token)
        {
            var endpointKey = NftModel.GetEndpointKey(Type);

            var client = await AjunaClientModel.GetOrAddSubstrateClientAsync(endpointKey);

            return await UniqueryPlus.Collections.CollectionModel.GetCollectionByCollectionIdAsync(client.SubstrateClient, Type, (uint)CollectionId, token).ConfigureAwait(false);
        }
        public async Task<INftBase> GetFullAsync(CancellationToken token)
        {
            var endpointKey = NftModel.GetEndpointKey(Type);

            var client = await AjunaClientModel.GetOrAddSubstrateClientAsync(endpointKey);

            var nft = await UniqueryPlus.Nfts.NftModel.GetNftByIdAsync(client.SubstrateClient, Type, (uint)CollectionId, (uint)Id, token).ConfigureAwait(false);

            return await nft.GetFullAsync(token);
        }
    }
    public record NftDatabaseItem
    {
        [PrimaryKey]
        [Unique]
        public string Key { get; set; } = "";
        public string SerializedNftBase { get; set; } = "";
        public string SerializedEndpoint { get; set; } = "";
        public bool Favourite { get; set; }

        public static implicit operator NftWrapper(NftDatabaseItem item)
        {
            Console.WriteLine("Serialized saved: ");
            Console.WriteLine(item.Key);
            Console.WriteLine(item.SerializedNftBase);

            var keyValues = item.Key.Split('-');

            if (keyValues.Length != 3)
            {
                throw new Exception("This should not happen");
            }

            var nftBase = JsonSerializer.Deserialize<SavedNftBase>(item.SerializedNftBase);

            nftBase.CollectionId = BigInteger.Parse(keyValues[1]);
            nftBase.Id = BigInteger.Parse(keyValues[2]);

            return new NftWrapper
            {
                NftBase = nftBase,
                Endpoint = JsonSerializer.Deserialize<Endpoint>(item.SerializedEndpoint),
                Favourite = item.Favourite,
            };
        }
    }

    public static class NftDatabase
    {
        private static NftDatabaseItem ToNftDatabaseItem(this NftWrapper wrapper) => new NftDatabaseItem
        {
            Key = $"{wrapper.Key.Value.Item1}-{wrapper.Key.Value.Item2}-{wrapper.Key.Value.Item3}",
            SerializedNftBase = JsonSerializer.Serialize(wrapper.NftBase),
            SerializedEndpoint = JsonSerializer.Serialize(wrapper.Endpoint),
            Favourite = wrapper.Favourite
        };

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private static SQLiteAsyncConnection Database; // Is never null after InitAsync
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private static async Task InitAsync()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(SQLiteConstants.NftDatabasePath, SQLiteConstants.NftDatabaseFlags);
            var result = await Database.CreateTableAsync<NftDatabaseItem>().ConfigureAwait(false);
        }

        public static async Task<IEnumerable<NftWrapper>> GetNftsAsync()
        {
            await InitAsync().ConfigureAwait(false);
            return (await Database.Table<NftDatabaseItem>().ToListAsync().ConfigureAwait(false)).Select(p => (NftWrapper)p);
        }

        public static async Task<IEnumerable<NftWrapper>> GetFavouriteNftsAsync()
        {
            await InitAsync().ConfigureAwait(false);
            return (await Database.Table<NftDatabaseItem>().Where(t => t.Favourite).ToListAsync().ConfigureAwait(false)).Select(p => (NftWrapper)p);
        }

        public static async Task<IEnumerable<NftWrapper>> GetNotFavouriteNftsOwnedByAsync(string address)
        {
            await InitAsync().ConfigureAwait(false);

            var saved = await Database.Table<NftDatabaseItem>().Where(t => !t.Favourite).ToListAsync().ConfigureAwait(false);

            return saved.Select(p => (NftWrapper)p).Where(t => t.NftBase != null && t.NftBase.Owner == address);
        }

        public static async Task<IEnumerable<NftWrapper>> GetNftsOwnedByAsync(string address)
        {
            await InitAsync().ConfigureAwait(false);

            var saved = await Database.Table<NftDatabaseItem>().ToListAsync().ConfigureAwait(false);

            return saved.Select(p => (NftWrapper)p).Where(t => t.NftBase != null && t.NftBase.Owner == address);
        }

        public static async Task<int> SaveItemAsync(NftWrapper item)
        {
            var databaseItem = item.ToNftDatabaseItem();

            await InitAsync().ConfigureAwait(false);

            var exists = (await Database.FindAsync<NftDatabaseItem>(databaseItem.Key).ConfigureAwait(false)) is not null;

            if (exists)
            {
                return await Database.UpdateAsync(databaseItem).ConfigureAwait(false);
            }
            else
            {
                return await Database.InsertAsync(databaseItem).ConfigureAwait(false);
            }
        }
        public static async Task<int> DeleteItemAsync(NftWrapper item)
        {
            await InitAsync().ConfigureAwait(false);
            return await Database.DeleteAsync(item.ToNftDatabaseItem()).ConfigureAwait(false);
        }

        public static async Task DeleteAllAsync()
        {
            await InitAsync().ConfigureAwait(false);
            await Database.DeleteAllAsync<NftDatabaseItem>().ConfigureAwait(false);
        }
    }
}

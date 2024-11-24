using PlutoWallet.Model.Constants;
using SQLite;
using System.Text.Json;
using UniqueryPlus.Collections;
using UniqueryPlus.Metadata;
using UniqueryPlus.Nfts;
using UniqueryPlus;
using System.Numerics;
using PlutoWallet.Constants;

namespace PlutoWallet.Model.SQLite
{
    public class SavedCollectionBase : ICollectionBase
    {
        public NftTypeEnum Type { get; set; }
        public BigInteger CollectionId { get; set; }
        public string Owner { get; set; }
        public uint NftCount { get; set; }
        public MetadataBase Metadata { get; set; }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IEnumerable<INftBase>> GetNftsAsync(uint limit, byte[]? lastKey, CancellationToken token)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var endpointKey = NftModel.GetEndpointKey(Type);

            var client = await AjunaClientModel.GetOrAddSubstrateClientAsync(endpointKey);

            if (NftCount == 0)
            {
                return [];
            }

            var result = await UniqueryPlus.Nfts.NftModel.GetNftsInCollectionOnChainAsync(client.SubstrateClient, Type, (uint)CollectionId, limit, lastKey, token).ConfigureAwait(false);

            return result.Items;
        }
        public async Task<IEnumerable<INftBase>> GetNftsOwnedByAsync(string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var endpointKey = NftModel.GetEndpointKey(Type);

            var client = await AjunaClientModel.GetOrAddSubstrateClientAsync(endpointKey);

            if (NftCount == 0)
            {
                return [];
            }

            var result = await UniqueryPlus.Nfts.NftModel.GetNftsInCollectionOwnedByOnChainAsync(client.SubstrateClient, Type, (uint)CollectionId, owner, limit, lastKey, token).ConfigureAwait(false);

            return result.Items;
        }
        public async Task<ICollectionBase> GetFullAsync(CancellationToken token)
        {
            var endpointKey = NftModel.GetEndpointKey(Type);

            var client = await AjunaClientModel.GetOrAddSubstrateClientAsync(endpointKey);

            var collectionBase = await UniqueryPlus.Collections.CollectionModel.GetCollectionByCollectionIdAsync(client.SubstrateClient, Type, (uint)CollectionId, token).ConfigureAwait(false);

            return await collectionBase.GetFullAsync(token);
        }
    }
    public record CollectionDatabaseItem
    {
        [PrimaryKey]
        [Unique]
        public string Key { get; set; } = "";
        public string SerializedCollectionBase { get; set; } = "";
        public string SerializedEndpoint { get; set; } = "";
        public bool Favourite { get; set; }

        public static implicit operator CollectionWrapper(CollectionDatabaseItem item)
        {
            var keyValues = item.Key.Split('-');

            if (keyValues.Length != 2)
            {
                throw new Exception("This should not happen");
            }

            var collectionBase = JsonSerializer.Deserialize<SavedCollectionBase>(item.SerializedCollectionBase);

            collectionBase.CollectionId = BigInteger.Parse(keyValues[1]);

            return new CollectionWrapper
            {
                CollectionBase = collectionBase,
                Endpoint = JsonSerializer.Deserialize<Endpoint>(item.SerializedEndpoint),
                Favourite = item.Favourite,
            };
        }
    }


    public static class CollectionDatabase
    {
        private static CollectionDatabaseItem ToCollectionDatabaseItem(this CollectionWrapper wrapper) => new CollectionDatabaseItem
        {
            Key = $"{wrapper.Key.Value.Item1}-{wrapper.Key.Value.Item2}",
            SerializedCollectionBase = JsonSerializer.Serialize(wrapper.CollectionBase),
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

            Database = new SQLiteAsyncConnection(SQLiteConstants.CollectionDatabasePath, SQLiteConstants.CollectionDatabaseFlags);
            var result = await Database.CreateTableAsync<CollectionDatabaseItem>().ConfigureAwait(false);
        }

        public static async Task<CollectionWrapper?> GetCollectionAsync(string key)
        {
            var collection = await Database.FindAsync<CollectionDatabaseItem>(key).ConfigureAwait(false);

            // This avoids implicit casting, which would lead to System.NullReferenceException
            if (collection is null)
            {
                return null;
            }

            return collection;
        }
        public static async Task<IEnumerable<CollectionWrapper>> GetCollectionsAsync()
        {
            await InitAsync().ConfigureAwait(false);
            return (await Database.Table<CollectionDatabaseItem>().ToListAsync().ConfigureAwait(false)).Select(p => (CollectionWrapper)p); ;
        }

        public static async Task<IEnumerable<CollectionWrapper>> GetFavouriteCollectionsAsync()
        {
            await InitAsync().ConfigureAwait(false);
            return (await Database.Table<CollectionDatabaseItem>().Where(t => t.Favourite).ToListAsync().ConfigureAwait(false)).Select(p => (CollectionWrapper)p);
        }

        public static async Task<IEnumerable<CollectionWrapper>> GetNotFavouriteCollectionsAsync()
        {
            await InitAsync().ConfigureAwait(false);
            return (await Database.Table<CollectionDatabaseItem>().Where(t => !t.Favourite).ToListAsync().ConfigureAwait(false)).Select(p => (CollectionWrapper)p);
        }
        public static async Task<IEnumerable<CollectionWrapper>> GetNotFavouriteCollectionsOwnedByAsync(string address)
        {
            await InitAsync().ConfigureAwait(false);

            var saved = await Database.Table<CollectionDatabaseItem>().Where(t => !t.Favourite).ToListAsync().ConfigureAwait(false);

            return saved.Select(p => (CollectionWrapper)p).Where(t => t.CollectionBase != null && t.CollectionBase.Owner == address);
        }
        public static async Task<IEnumerable<CollectionWrapper>> GetCollectionsOwnedByAsync(string address)
        {
            await InitAsync().ConfigureAwait(false);

            var saved = await Database.Table<CollectionDatabaseItem>().ToListAsync().ConfigureAwait(false);
            
            return saved.Select(p => (CollectionWrapper)p).Where(t => t.CollectionBase != null && t.CollectionBase.Owner == address);
        }
        public static async Task<int> SaveItemAsync(CollectionWrapper item)
        {
            var databaseItem = item.ToCollectionDatabaseItem();

            await InitAsync().ConfigureAwait(false);

            var exists = (await Database.FindAsync<CollectionDatabaseItem>(databaseItem.Key).ConfigureAwait(false)) is not null;

            Console.WriteLine("Adding: " + databaseItem.Key);

            if (exists)
            {
                return await Database.UpdateAsync(item.ToCollectionDatabaseItem()).ConfigureAwait(false);
            }
            else
            {
                return await Database.InsertAsync(databaseItem).ConfigureAwait(false);
            }
        }
        public static async Task<int> DeleteItemAsync(CollectionWrapper item)
        {
            await InitAsync().ConfigureAwait(false);
            return await Database.DeleteAsync(item.ToCollectionDatabaseItem()).ConfigureAwait(false);
        }

        public static async Task DeleteAllAsync()
        {
            await InitAsync().ConfigureAwait(false);
            await Database.DeleteAllAsync<CollectionDatabaseItem>().ConfigureAwait(false);
        }
    }
}

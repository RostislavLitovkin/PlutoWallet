
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using StreamJsonRpc;
using Bajun.NetApi.Generated;
using Bajun.NetApi.Generated.Model.frame_system;
using Bajun.NetApi.Generated.Storage;
using Bajun.NetApi.Helper;
using Substrate.NET.Schnorrkel.Keys;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;

namespace Bajun.NetApi.Client
{
    /// <summary>
    /// Base client
    /// </summary>
    public class BaseClient
    {
        private readonly int _maxConcurrentCalls;

        private readonly ChargeType _chargeTypeDefault;

        private static MiniSecret MiniSecretAlice => new MiniSecret(Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a"), ExpandMode.Ed25519);

        /// <summary>
        /// Alice account
        /// </summary>
        public static Account Alice => Account.Build(KeyType.Sr25519, MiniSecretAlice.ExpandToSecret().ToEd25519Bytes(), MiniSecretAlice.GetPair().Public.Key);

        /// <summary>
        /// Extrinsic manager, used to manage extrinsic subscriptions and the corresponding states.
        /// </summary>
        public ExtrinsicManager ExtrinsicManager { get; }

        /// <summary>
        /// Subscription manager, used to manage subscriptions of storage elements.
        /// </summary>
        public SubscriptionManager SubscriptionManager { get; }

        /// <summary>
        /// Substrate Extension Client
        /// </summary>
        public SubstrateClientExt SubstrateClient { get; }

        /// <summary>
        /// Is connected to the network
        /// </summary>
        public bool IsConnected => SubstrateClient.IsConnected;

        /// <summary>
        /// Base Client Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="maxConcurrentCalls"></param>
        public BaseClient(string url, int maxConcurrentCalls = 10)
        {
            _chargeTypeDefault = ChargeTransactionPayment.Default();

            _maxConcurrentCalls = maxConcurrentCalls;

            SubstrateClient = new SubstrateClientExt(new Uri(url), _chargeTypeDefault);

            ExtrinsicManager = new ExtrinsicManager();

            SubscriptionManager = new SubscriptionManager();
        }

        /// <summary>
        /// Connect to the network
        /// </summary>
        /// <param name="useMetadata"></param>
        /// <param name="standardSubstrate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(bool useMetadata, bool standardSubstrate, CancellationToken token)
        {
            if (!IsConnected)
            {
                try
                {
                    await SubstrateClient.ConnectAsync(useMetadata, standardSubstrate, token);
                }
                catch (Exception e)
                {
                    Log.Error("BaseClient.ConnectAsync: {0}",
                        e.ToString());
                }
            }

            return IsConnected;
        }

        /// <summary>
        /// Disconnect from the network
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DisconnectAsync()
        {
            if (!IsConnected)
            {
                return false;
            }

            await SubstrateClient.CloseAsync();
            return true;
        }

        /// <summary>
        /// Check if extrinsic can be sent
        /// </summary>
        /// <param name="extrinsicType"></param>
        /// <param name="concurrentTasks"></param>
        /// <returns></returns>
        public bool CanExtrinsic(string extrinsicType, int concurrentTasks)
            => IsConnected && !HasMaxConcurentTaskRunning() && !HasToManyConcurentTaskRunning(extrinsicType, concurrentTasks);

        /// <summary>
        /// Check if we have maximum of concurrent tasks running reached
        /// </summary>
        /// <returns></returns>
        public bool HasMaxConcurentTaskRunning()
            => ExtrinsicManager.Running.Count() >= _maxConcurrentCalls;

        /// <summary>
        /// Check if we have maximum of concurrent tasks running reached
        /// </summary>
        /// <param name="extrinsicType"></param>
        /// <param name="concurrentTasks"></param>
        /// <returns></returns>
        public bool HasToManyConcurentTaskRunning(string extrinsicType, int concurrentTasks)
            => ExtrinsicManager.Running.Count(p => p.ExtrinsicType == extrinsicType) >= concurrentTasks;

        /// <summary>
        /// Generic extrinsic sender
        /// </summary>
        /// <param name="extrinsicType"></param>
        /// <param name="extrinsicMethod"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GenericExtrinsicAsync(Account account, string extrinsicType, Method extrinsicMethod, int concurrentTasks, CancellationToken token)
        {
            if (account == null)
            {
                Log.Warning("Account is null!");
                return null;
            }

            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            if (HasMaxConcurentTaskRunning())
            {
                Log.Warning("There can not be more then {0} concurrent tasks overall!", _maxConcurrentCalls);
                return null;
            }

            if (HasToManyConcurentTaskRunning(extrinsicType, concurrentTasks))
            {
                Log.Warning("There can not be more then {0} concurrent tasks of {1}!", concurrentTasks, extrinsicType);
                return null;
            }

            string subscription = null;
            try
            {
                subscription = await SubstrateClient.TransactionWatchCalls.TransactionWatchV1SubmitAndWatchAsync(ActionExtrinsicUpdate, extrinsicMethod, account, _chargeTypeDefault, 64, token);
            }
            catch (RemoteInvocationException e)
            {
                Log.Error("RemoteInvocationException: {0}", e.Message);
                return subscription;
            }

            if (subscription == null)
            {
                return null;
            }

            Log.Debug("Generic extrinsic sent {0} with {1}.", extrinsicMethod.ModuleName + "_" + extrinsicMethod.CallName, subscription);

            if (ExtrinsicManager.TryAdd(subscription, extrinsicType))
            {
                Log.Debug("Generic extrinsic sent {0} with {1}.", extrinsicMethod.ModuleName + "_" + extrinsicMethod.CallName, subscription);
            }
            else
            {
                Log.Warning("ExtrinsicManager.Add failed for {0} with {1}.", extrinsicMethod.ModuleName + "_" + extrinsicMethod.CallName, subscription);
            }

            return subscription;
        }

        /// <summary>
        /// Callback for extrinsic update
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="extrinsicUpdate"></param>
        public async void ActionExtrinsicUpdate(string subscriptionId, TransactionEventInfo extrinsicUpdate)
        {
            try
            {
                ExtrinsicManager.UpdateExtrinsicInfo(subscriptionId, extrinsicUpdate);

                // proccessing events scrapping
                if (ExtrinsicManager.TryGet(subscriptionId, out ExtrinsicInfo extrinsicInfo) && !extrinsicInfo.HasEvents && extrinsicUpdate.Hash != null && extrinsicUpdate.Index != null)
                {
                    string parameters = SystemStorage.EventsParams();

                    BaseVec<EventRecord> events = await SubstrateClient.GetStorageAsync<BaseVec<EventRecord>>(parameters, extrinsicUpdate.Hash.Value, CancellationToken.None);
                    if (events == null)
                    {
                        ExtrinsicManager.UpdateExtrinsicError(subscriptionId, "No block events");
                        return;
                    }

                    System.Collections.Generic.IEnumerable<EventRecord> allExtrinsicEvents = events.Value.Where(p => p.Phase.Value == Phase.ApplyExtrinsic && ((U32)p.Phase.Value2).Value == extrinsicUpdate.Index);
                    if (!allExtrinsicEvents.Any())
                    {
                        ExtrinsicManager.UpdateExtrinsicError(subscriptionId, "No extrinsic events");
                        return;
                    }

                    ExtrinsicManager.UpdateExtrinsicEvents(subscriptionId, allExtrinsicEvents);
                }
            }
            catch (Exception ex)
            {
                Log.Warning("ActionExtrinsicUpdate: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Subscribe to event storage
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> SubscribeEventsAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            if (SubscriptionManager.IsSubscribed)
            {
                Log.Warning("Already active subscription to events!");
                return null;
            }

            string subscription = await SubstrateClient.SubscribeStorageKeyAsync(SystemStorage.EventsParams(), SubscriptionManager.ActionSubscrptionEvent, token);
            if (subscription == null)
            {
                return null;
            }

            SubscriptionManager.IsSubscribed = true;

            Log.Debug("SystemStorage.Events subscription id [{0}] registred.", subscription);

            return subscription;
         }

         /// <summary>
         /// Generate a random account
         /// </summary>
         /// <param name="seed"></param>
         /// <param name="derivationPsw"></param>
         /// <param name="keyType"></param>
         /// <returns></returns>
         public static Account RandomAccount(int seed, string derivationPsw = "", KeyType keyType = KeyType.Sr25519)
         {
            var random = new Random(seed);
            byte[] randomBytes = new byte[16];
            random.NextBytes(randomBytes);
            string mnemonic = string.Join(" ", Mnemonic.MnemonicFromEntropy(randomBytes, Mnemonic.BIP39Wordlist.English));
            Log.Debug("mnemonic[Sr25519]: {0} ", mnemonic);
            return Mnemonic.GetAccountFromMnemonic(mnemonic, derivationPsw, keyType);
         }
      }
}


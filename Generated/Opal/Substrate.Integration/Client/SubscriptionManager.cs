
using System;
using Serilog;
using Substrate.NetApi.Model.Rpc;
using Substrate.Integration.Helper;

namespace Substrate.Integration.Client
{
    public delegate void SubscriptionOnEvent(string subscriptionId, StorageChangeSet storageChangeSet);

    public class SubscriptionManager
    {
        public bool IsSubscribed { get; set; }

        public event SubscriptionOnEvent SubscrptionEvent;

        public SubscriptionManager()
        {
            SubscrptionEvent += OnSystemEvents;
        }

        /// <summary>
        /// Simple extrinsic tester
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="storageChangeSet"></param>
        public void ActionSubscrptionEvent(string subscriptionId, StorageChangeSet storageChangeSet)
        {
            IsSubscribed = false;

            Log.Information("System.Events: {0}", storageChangeSet);

            SubscrptionEvent?.Invoke(subscriptionId, storageChangeSet);
        }

        /// <summary>
        /// Handle system events
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="storageChangeSet"></param>
        private void OnSystemEvents(string subscriptionId, StorageChangeSet storageChangeSet)
        {
            Log.Debug("OnExtrinsicUpdated[{id}] updated {state}",
                subscriptionId,
                storageChangeSet);
        }
    }
}


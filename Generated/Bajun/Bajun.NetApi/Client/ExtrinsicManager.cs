
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Substrate.NetApi.Model.Rpc;
using Bajun.NetApi.Generated.Model.frame_system;
using Bajun.NetApi.Generated;
using Bajun.NetApi.Helper;
using Serilog;

namespace Bajun.NetApi.Client
{
    public delegate void ExtrinsicUpdateEvent(string subscriptionId, ExtrinsicInfo queueInfo);

    public class ExtrinsicManager
    {
        public event ExtrinsicUpdateEvent ExtrinsicUpdated;

        public IEnumerable<ExtrinsicInfo> Running => _data.Values.Where(p => !p.IsCompleted && !p.IsTimeout);

        public IEnumerable<ExtrinsicInfo> PreInblock => _data.Values.Where(p => !p.IsInBlock && !p.IsCompleted && !p.IsTimeout);

        private readonly ConcurrentDictionary<string, ExtrinsicInfo> _data;

        private readonly int _retentationTimeSec;

        private readonly int _extrinsicTimeOut;

        /// <summary>
        /// Extrinsic manager
        /// </summary>
        /// <param name="extrinsicTimeOut"></param>
        /// <param name="retentationTime"></param>
        public ExtrinsicManager(int extrinsicTimeOut = 30, int retentationTime = 60)
        {
            _data = new ConcurrentDictionary<string, ExtrinsicInfo>();
            _retentationTimeSec = retentationTime;
            _extrinsicTimeOut = extrinsicTimeOut;
            ExtrinsicUpdated += OnExtrinsicUpdated;
        }

        /// <summary>
        /// Try to add a new extrinsic to the manager.
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="extrinsicType"></param>
        public bool TryAdd(string subscription, string extrinsicType)
        {
            return _data.TryAdd(subscription, new ExtrinsicInfo(extrinsicType, _extrinsicTimeOut));
        }

        /// <summary>
        /// Get extrinsic info by subscriptionId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extrinsicInfo"></param>
        /// <returns></returns>
        public bool TryGet(string id, out ExtrinsicInfo extrinsicInfo)
        {
            if (!_data.TryGetValue(id, out extrinsicInfo))
            {
                Log.Debug("ExtrinsicInfo not available for subscriptionId {id}", id);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Update extrinsic info.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="extrinsicUpdate"></param>
        public void UpdateExtrinsicInfo(string subscriptionId, TransactionEventInfo extrinsicUpdate)
        {
            if (!_data.TryGetValue(subscriptionId, out ExtrinsicInfo queueInfo) || queueInfo == null)
            {
                queueInfo = new ExtrinsicInfo("Unknown", _extrinsicTimeOut);
            }
            queueInfo.Update(extrinsicUpdate);

            /// Possible transaction status events.
            ///
            /// The status events can be grouped based on their kinds as:
            ///
            /// 1. Runtime validated the transaction:
            /// 		- `Validated`
            ///
            /// 2. Inside the `Ready` queue:
            /// 		- `Broadcast`
            ///
            /// 3. Leaving the pool:
            /// 		- `BestChainBlockIncluded`
            /// 		- `Invalid`
            ///
            /// 4. Block finalized:
            /// 		- `Finalized`
            ///
            /// 5. At any time:
            /// 		- `Dropped`
            /// 		- `Error`
            ///
            /// The subscription's stream is considered finished whenever the following events are
            /// received: `Finalized`, `Error`, `Invalid` or `Dropped`. However, the user is allowed
            /// to unsubscribe at any moment.

            ExtrinsicUpdated?.Invoke(subscriptionId, queueInfo);

            if (!queueInfo.HasEvents && queueInfo.Hash != null && queueInfo.Index != null)
            {
                Log.Debug("Extrinsic {id} completed with {state}", subscriptionId, queueInfo.TransactionEvent);
            }

            CleanUp(false);
        }

        /// <summary>
        /// Clean up completed and time outed extrinsics.
        /// </summary>
        /// <param name="timeOut"></param>
        public void CleanUp(bool timeOut)
        {
            var removeKeys = _data
                .Where(p => (p.Value.TimeElapsed > _retentationTimeSec && p.Value.IsCompleted) || (timeOut && p.Value.IsTimeout))
                .Select(p => p.Key)
                .ToList();

            Log.Debug("Remove {count} completed and time outed extrinsics, after {time}", removeKeys.Count, _retentationTimeSec);

            foreach (string key in removeKeys)
            {
                _data.TryRemove(key, out _);
            }
        }

        /// <summary>
        /// Update extrinsic events.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="allExtrinsicEvents"></param>
        internal void UpdateExtrinsicEvents(string subscriptionId, IEnumerable<EventRecord> allExtrinsicEvents)
        {
            if (!_data.TryGetValue(subscriptionId, out ExtrinsicInfo queueInfo))
            {
                return;
            }

            queueInfo.EventRecords = allExtrinsicEvents.ToList();
            ExtrinsicUpdated?.Invoke(subscriptionId, queueInfo);
        }

        /// <summary>
        /// Update extrinsic error.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="errorMsg"></param>
        internal void UpdateExtrinsicError(string subscriptionId, string errorMsg)
        {
            if (!_data.TryGetValue(subscriptionId, out ExtrinsicInfo queueInfo))
            {
                return;
            }

            queueInfo.Error = errorMsg;
            ExtrinsicUpdated?.Invoke(subscriptionId, queueInfo);
        }

        /// <summary>
        /// Simple extrinsic tester
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="queueInfo"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnExtrinsicUpdated(string subscriptionId, ExtrinsicInfo queueInfo)
        {
            Log.Debug("{name}[{id}] updated {state}",
                queueInfo.ExtrinsicType,
                subscriptionId,
                queueInfo.TransactionEvent);
        }
    }
}


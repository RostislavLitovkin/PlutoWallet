
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApiExt.Generated.Model.primitive_types;
using Substrate.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using Substrate.NetApiExt.Generated.Model.frame_support.dispatch;
using Substrate.NetApiExt.Generated.Model.frame_system;
using Substrate.NetApiExt.Generated.Model.frame_system.pallet;
using Substrate.NetApiExt.Generated.Model.sp_arithmetic;
using Substrate.NetApiExt.Generated.Model.sp_runtime;
using Substrate.Integration.Helper;
using Substrate.NetApiExt.Generated.Model.bajun_runtime;

namespace Substrate.Integration.Client
{
    public class ExtrinsicInfo
    {
        public int ExtrinsicTimeOutSec { get; }

        public TransactionEvent? TransactionEvent { get; private set; }

        public string ExtrinsicType { get; }

        public DateTime Created { get; }

        public DateTime LastUpdated { get; private set; }

        public Hash Hash { get; private set; }

        public uint? Index { get; set; }

        public bool IsReady { get; private set; }

        public bool IsInBlock { get; private set; }

        public bool IsSuccess { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsTimeout => TimeElapsed > ExtrinsicTimeOutSec;

        public bool HasEvents => EventRecords != null;

        public string Error { get; set; }

        public List<EventRecord> EventRecords { get; set; }

        public double TimeElapsed => DateTime.UtcNow.Subtract(LastUpdated).TotalSeconds;

        public ExtrinsicInfo(string extrinsicType, int timeOutSec)
        {
            ExtrinsicTimeOutSec = timeOutSec;
            ExtrinsicType = extrinsicType;
            Created = DateTime.UtcNow;
            LastUpdated = Created;
            TransactionEvent = null;
            Hash = null;
            IsReady = false;
            IsInBlock = false;
            IsSuccess = false;
            IsCompleted = false;

            EventRecords = null;
        }

        internal void Update(TransactionEventInfo transactionEventInfo)
        {
            LastUpdated = DateTime.UtcNow;

            TransactionEvent = transactionEventInfo.TransactionEvent;
            Hash = transactionEventInfo.Hash;
            Index = transactionEventInfo.Index;
            Error = transactionEventInfo.Error;

            switch (TransactionEvent)
            {
                case NetApi.Model.Rpc.TransactionEvent.Validated:
                    IsReady = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Broadcasted:
                    break;

                case NetApi.Model.Rpc.TransactionEvent.BestChainBlockIncluded:
                    IsInBlock = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Finalized:
                    IsSuccess = true;
                    IsCompleted = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Error:
                    IsCompleted = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Invalid:
                    IsCompleted = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Dropped:
                    IsCompleted = true;
                    break;

                default:
                    throw new NotSupportedException($"Unknown TransactionEvent {TransactionEvent}");
            }
        }

        public bool SystemExtrinsicEvent(out Event? systemExtrinsicEvent, out string errorMsg)
        {
            systemExtrinsicEvent = null;
            errorMsg = null;

            if (!AllEvents(RuntimeEvent.System, out IEnumerable<EnumEvent> allEnumEvents))
            {
                return false;
            }

            EnumEvent systemEnumEvent = allEnumEvents.LastOrDefault();
            if (systemEnumEvent == null)
            {
                return false;
            }

            switch (systemEnumEvent.Value)
            {
                case Event.ExtrinsicSuccess:
                    break;

                case Event.ExtrinsicFailed:
                    var systemEnumEventData = (BaseTuple<EnumDispatchError, DispatchInfo>)systemEnumEvent.Value2;
                    var enumDispatchError = (EnumDispatchError)systemEnumEventData.Value[0];
                    errorMsg = MessageFromDispatchError(enumDispatchError);
                    break;

                default:
                    return false;
            }

            systemExtrinsicEvent = systemEnumEvent.Value;
            return true;
        }

        private string MessageFromDispatchError(EnumDispatchError dispatchError)
        {
            switch (dispatchError.Value)
            {
                case DispatchError.Module:
                    var moduleError = (ModuleError)dispatchError.Value2;
                    return $"{dispatchError.Value};{(RuntimeEvent)moduleError.Index.Value};{moduleError.Index.Value};{Utils.Bytes2HexString(moduleError.Error.Value.ToBytes())}";

                case DispatchError.Token:
                    var enumTokenError = (EnumTokenError)dispatchError.Value2;
                    return $"{dispatchError.Value};{enumTokenError.Value}";

                case DispatchError.Arithmetic:
                    var enumArithmeticError = (EnumArithmeticError)dispatchError.Value2;
                    return $"{dispatchError.Value};{enumArithmeticError.Value}";

                case DispatchError.Transactional:
                    var enumTransactionalError = (EnumTransactionalError)dispatchError.Value2;
                    return $"{dispatchError.Value};{enumTransactionalError.Value}";

                default:
                    return dispatchError.Value.ToString();
            }
        }

        public bool AllEvents<T>(RuntimeEvent runtimeEvent, out IEnumerable<T> allEnumEvents)
        {
            allEnumEvents = null;

            if (EventRecords == null || !EventRecords.Any())
            {
                return false;
            }

            IEnumerable<EventRecord> allevents = EventRecords.Where(p => p.Event.Value == runtimeEvent);
            if (!allevents.Any())
            {
                return false;
            }

            allEnumEvents = allevents.Select(p => (T)p.Event.Value2);
            return true;
        }
    }
}


using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;

namespace PlutoWallet.Model
{
    public class ExtrinsicDetails
    {
        public IEnumerable<ExtrinsicEvent> Events { get; set; }
        public BigInteger BlockNumber { get; set; }
        public uint? ExtrinsicIndex { get; set; }
    }
    public enum EventSafety
    {
        NothingUpdateUIBug,
        Safe,
        Ok,
        Unknown,
        Warning,
        Harmful,
    }
    public class ExtrinsicEvent
    {
        public string PalletName { get; set; }
        public string EventName { get; set; }
        public object? Parameters { get; set; }
        public EventSafety Safety { get; set; }

        public ExtrinsicEvent(string palletName, string eventName, object? parameters)
        {
            PalletName = palletName;
            EventName = eventName;
            Parameters = parameters;

            SetSafety();
        }

        private void SetSafety()
        {
            Safety = (PalletName, EventName, Parameters) switch
            {
                ("System", "ExtrinsicSuccess", _) => EventSafety.Safe,
                ("System", "ExtrinsicFailed", _) => EventSafety.Harmful,
                ("System", _, _) => EventSafety.Ok,
                ("Balances", "Deposit", _) => EventSafety.Safe,
                ("Balances", _, _) => EventSafety.Warning,
                ("Assets", _, _) => EventSafety.Warning,
                ("Tokens", _, _) => EventSafety.Warning,
                ("PolkadotXcm", _, _) => EventSafety.Warning,
                _ => EventSafety.Unknown,
            };
        }
    }

    public class EventParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public static class EventsModel
    {

        public static List<EventParameter> GetParametersList(object parameters)
        {
            if (parameters == null)
            {
                return new List<EventParameter>();
            }

            var parametersList = new List<EventParameter>();

            var pValue = parameters.GetProperty("Value");

            if (pValue == null || !(pValue is IType[]))
            {
                return new List<EventParameter>();
            }

            foreach (var parameter in (IType[])pValue)
            {
                Type type = parameter.GetType();

                if (type.Name == "AccountId32")
                {
                    var accountIdEncoded = ((IType)parameter.GetProperty("Value")).Encode();
                    string address = Utils.GetAddressFrom(accountIdEncoded);
                    parametersList.Add(new EventParameter
                    {
                        Name = type.Name,
                        Value = address
                    });

                }
                else
                {
                    parametersList.Add(new EventParameter
                    {
                        Name = type.Name,
                        Value = parameter.ToString(),
                    });
                }
            }

            return parametersList;
        }

        public static object? GetProperty<T>(
            this T t,
            string propertyName
            )
        {
            return t?.GetType().GetProperty(propertyName)?.GetValue(t);
        }

        public static string GetValueString<T>(
            this T t,
            string propertyName = "Value"
            )
        {
            return t?.GetType().GetProperty(propertyName)?.GetValue(t)?.ToString() ?? "Unknown";
        }

        /// <summary>
        /// Gets all extrinsic events in the block
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="blockHash"></param>
        /// <param name="extrinsicHash"></param>
        /// <returns>all events for the given extrinsic</returns>
        /// <exception cref="ExtrinsicIndexNotFoundException"></exception>
        public static async Task<ExtrinsicDetails> GetExtrinsicEventsAsync(
            this SubstrateClientExt substrateClient,
           EndpointEnum endpointKey,
            Hash blockHash,
            byte[] extrinsicHash,
            CancellationToken token = default
        )
        {
            return endpointKey switch
            {
                EndpointEnum.Polkadot => await GetExtrinsicEventsAsync<Polkadot.NetApi.Generated.Model.polkadot_runtime.EnumRuntimeEvent>(substrateClient, blockHash, extrinsicHash, token),
                EndpointEnum.PolkadotAssetHub => await GetExtrinsicEventsAsync<PolkadotAssetHub.NetApi.Generated.Model.asset_hub_polkadot_runtime.EnumRuntimeEvent>(substrateClient, blockHash, extrinsicHash, token),
                EndpointEnum.Opal => await GetExtrinsicEventsAsync<Opal.NetApi.Generated.Model.opal_runtime.EnumRuntimeEvent>(substrateClient, blockHash, extrinsicHash, token),
                EndpointEnum.Hydration => await GetExtrinsicEventsAsync<Hydration.NetApi.Generated.Model.hydradx_runtime.EnumRuntimeEvent>(substrateClient, blockHash, extrinsicHash, token),
                _ => await GetExtrinsicDetailsAsync(substrateClient, blockHash, extrinsicHash, token),
            };
        }

        /// <summary>
        /// Gets all extrinsic events in the block
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="blockHash"></param>
        /// <param name="unCheckedExtrinsic"></param>
        /// <returns>all events for the given extrinsic</returns>
        public static async Task<ExtrinsicDetails> GetExtrinsicEventsAsync<T>(
            this SubstrateClientExt substrateClient,
            Hash blockHash,
            byte[] extrinsicHash,
            CancellationToken token = default
        ) where T : BaseEnumType, new()
        {
            string blockHashString = Utils.Bytes2HexString(blockHash);

            var eventsParameters = RequestGenerator.GetStorage("System", "Events", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
            var events = await substrateClient.SubstrateClient.GetStorageAsync<BaseVec<UniversalEventRecord<T>>>(eventsParameters, blockHashString, token);

            BlockData block = await substrateClient.SubstrateClient.Chain.GetBlockAsync(blockHash, CancellationToken.None);

            uint? extrinsicIndex = null;
            for (uint i = 0; i < block.Block.Extrinsics.Count(); i++)
            {
                // Same extrinsic
                if (Utils.Bytes2HexString(HashExtension.Blake2(block.Block.Extrinsics[i].Encode(), 256)).Equals(Utils.Bytes2HexString(extrinsicHash)))
                {
                    extrinsicIndex = i;
                    break;
                }
            };

            if (extrinsicIndex is null || events is null)
            {
                return new ExtrinsicDetails
                {
                    BlockNumber = block.Block.Header.Number.Value,
                    ExtrinsicIndex = extrinsicIndex,
                    Events = new List<ExtrinsicEvent>()
                };
            }

            var sortedEvents = events.Value.Where(p => p.Phase.Value == Substrate.NetApi.Generated.Model.frame_system.Phase.ApplyExtrinsic && ((U32)p.Phase.Value2).Value == extrinsicIndex);


            return new ExtrinsicDetails
            {
                BlockNumber = block.Block.Header.Number.Value,
                ExtrinsicIndex = extrinsicIndex,
                Events = sortedEvents.Select(e =>
                {
                    string palletName = e.Event.GetValueString();
                    object? eventValue2 = e.Event.GetProperty("Value2");
                    string eventName = eventValue2.GetValueString();
                    object? parameters = eventValue2.GetProperty("Value2");

                    return new ExtrinsicEvent(palletName, eventName, parameters);
                })
            };
        }

        /// <summary>
        /// Gets extrinsic details without events
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="blockHash"></param>
        /// <param name="extrinsicHash"></param>
        /// <returns>all events for the given extrinsic</returns>
        public static async Task<ExtrinsicDetails> GetExtrinsicDetailsAsync(
            this SubstrateClientExt substrateClient,
            Hash blockHash,
            byte[] extrinsicHash,
            CancellationToken token = default
        )
        {
            string blockHashString = Utils.Bytes2HexString(blockHash);

            var eventsParameters = RequestGenerator.GetStorage("System", "Events", Substrate.NetApi.Model.Meta.Storage.Type.Plain);

            BlockData block = await substrateClient.SubstrateClient.Chain.GetBlockAsync(blockHash, CancellationToken.None);

            uint? extrinsicIndex = null;
            for (uint i = 0; i < block.Block.Extrinsics.Count(); i++)
            {
                // Same extrinsic
                if (Utils.Bytes2HexString(HashExtension.Blake2(block.Block.Extrinsics[i].Encode(), 256)).Equals(Utils.Bytes2HexString(extrinsicHash)))
                {
                    extrinsicIndex = i;
                    break;
                }
            };

            return new ExtrinsicDetails
            {
                BlockNumber = block.Block.Header.Number.Value,
                ExtrinsicIndex = extrinsicIndex,
                Events = new List<ExtrinsicEvent>()
            };
        }
    }

    public sealed class UniversalEventRecord<T> : BaseType where T : BaseEnumType, new()
    {
        /// <summary>
        /// >> phase
        /// </summary>
        private Substrate.NetApi.Generated.Model.frame_system.EnumPhase _phase;

        /// <summary>
        /// >> event
        /// </summary>
        private T _event;

        /// <summary>
        /// >> topics
        /// </summary>
        private Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Generated.Model.primitive_types.H256> _topics;

        public Substrate.NetApi.Generated.Model.frame_system.EnumPhase Phase
        {
            get
            {
                return this._phase;
            }
            set
            {
                this._phase = value;
            }
        }

        public T Event
        {
            get
            {
                return this._event;
            }
            set
            {
                this._event = value;
            }
        }

        public BaseVec<Substrate.NetApi.Generated.Model.primitive_types.H256> Topics
        {
            get
            {
                return this._topics;
            }
            set
            {
                this._topics = value;
            }
        }

        public override string TypeName()
        {
            return "EventRecord";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Phase.Encode());
            result.AddRange(Event.Encode());
            result.AddRange(Topics.Encode());
            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Phase = new Substrate.NetApi.Generated.Model.frame_system.EnumPhase();
            Phase.Decode(byteArray, ref p);
            Event = new T();
            Event.Decode(byteArray, ref p);
            Topics = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Generated.Model.primitive_types.H256>();
            Topics.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}

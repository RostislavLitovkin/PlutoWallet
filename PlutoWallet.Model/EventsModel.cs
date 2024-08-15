using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Types;
using Substrate.NetApi;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Collections.Generic;
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
        public List<EventParameter> Parameters { get; set; }
        public EventSafety Safety { get; set; }

        public ExtrinsicEvent(string palletName, string eventName, List<EventParameter> parameters)
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

        public static List<EventParameter> GetParametersList(object parameters, TypeField[] eventTypeFields)
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

            var pValues = (IType[])pValue;

            for(int i = 0; i < pValues.Length; i++)
            {
                var parameter = pValues[i];
                var eventTypeField = eventTypeFields[i];

                Type type = parameter.GetType();

                if (type.Name == "AccountId32")
                {
                    var accountIdEncoded = ((IType)parameter.GetProperty("Value")).Encode();
                    string address = Utils.GetAddressFrom(accountIdEncoded);
                    parametersList.Add(new EventParameter
                    {
                        Name = eventTypeField.Name,
                        Value = address
                    });

                }
                else
                {
                    parametersList.Add(new EventParameter
                    {
                        Name = eventTypeField.Name,
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
        /// <returns>all events for the given extrinsic</returns>
        /// <exception cref="ExtrinsicIndexNotFoundException"></exception>
        public static async Task<ExtrinsicDetails> GetExtrinsicEventsAsync(
            this SubstrateClientExt substrateClient,
            Hash blockHash,
            byte[] extrinsicHash,
            CancellationToken token = default
        )
        {
            string blockHashString = Utils.Bytes2HexString(blockHash);

            var eventsParameters = RequestGenerator.GetStorage("System", "Events", Substrate.NetApi.Model.Meta.Storage.Type.Plain);

            string eventsBytes = await substrateClient.SubstrateClient.InvokeAsync<string>("state_getStorage", new object[2] { eventsParameters, blockHashString }, token);

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

            return await GetExtrinsicEventsForClientAsync(substrateClient, extrinsicIndex, eventsBytes, blockNumber: block.Block.Header.Number.Value, token);
        }

        public static async Task<ExtrinsicDetails> GetExtrinsicEventsForClientAsync(
            this SubstrateClientExt substrateClient,
            uint? extrinsicIndex,
            string? eventsBytes,
            BigInteger blockNumber,
            CancellationToken token
        )
        {
            return substrateClient.Endpoint.Key switch
            {
                EndpointEnum.Polkadot => await GetExtrinsicEventsAsync<Polkadot.NetApi.Generated.Model.polkadot_runtime.EnumRuntimeEvent>(substrateClient, extrinsicIndex, eventsBytes, blockNumber, token),
                EndpointEnum.PolkadotAssetHub => await GetExtrinsicEventsAsync<PolkadotAssetHub.NetApi.Generated.Model.asset_hub_polkadot_runtime.EnumRuntimeEvent>(substrateClient, extrinsicIndex, eventsBytes, blockNumber, token),
                EndpointEnum.Opal => await GetExtrinsicEventsAsync<Opal.NetApi.Generated.Model.opal_runtime.EnumRuntimeEvent>(substrateClient, extrinsicIndex, eventsBytes, blockNumber, token),
                EndpointEnum.Hydration => await GetExtrinsicEventsAsync<Hydration.NetApi.Generated.Model.hydradx_runtime.EnumRuntimeEvent>(substrateClient, extrinsicIndex, eventsBytes, blockNumber, token),
                _ => await GetExtrinsicDetailsAsync(substrateClient, extrinsicIndex, eventsBytes, blockNumber, token),
            };
        }

        /// <summary>
        /// Gets all extrinsic events in the block
        /// </summary>
        /// <returns>all events for the given extrinsic</returns>
        public static async Task<ExtrinsicDetails> GetExtrinsicEventsAsync<T>(
            this SubstrateClientExt substrateClient,
            uint? extrinsicIndex,
            string? eventsBytes,
            BigInteger blockNumber,
            CancellationToken token
        ) where T : BaseEnumType, new()
        {
            if (extrinsicIndex is null || eventsBytes == null || eventsBytes.Length == 0)
            {
                return new ExtrinsicDetails
                {
                    BlockNumber = blockNumber,
                    ExtrinsicIndex = extrinsicIndex,
                    Events = new List<ExtrinsicEvent>()
                };
            }

            var events = new BaseVec<UniversalEventRecord<T>>();
            events.Create(eventsBytes);

            var sortedEvents = events.Value.Where(p => p.Phase.Value == Substrate.NetApi.Generated.Model.frame_system.Phase.ApplyExtrinsic && ((U32)p.Phase.Value2).Value == extrinsicIndex);

            return new ExtrinsicDetails
            {
                BlockNumber = blockNumber,
                ExtrinsicIndex = extrinsicIndex,
                Events = sortedEvents.Select(e =>
                {
                    byte palletIndex = Convert.ToByte(e.Event.GetProperty("Value"));
                    string palletName = e.Event.GetValueString();
                    object? eventValue2 = e.Event.GetProperty("Value2");
                    byte eventIndex = Convert.ToByte(eventValue2.GetProperty("Value"));
                    string eventName = eventValue2.GetValueString();
                    object? parameters = eventValue2.GetProperty("Value2");

                    string _palletName = substrateClient.CustomMetadata.NodeMetadata.Modules[palletIndex.ToString()].Name;

                    TypeField[]? eventTypeFields = null;

                    foreach (var variant in substrateClient.CustomMetadata.NodeMetadata.Types[substrateClient.CustomMetadata.NodeMetadata.Modules[palletIndex.ToString()].Events.TypeId.ToString()].Variants)
                    {
                        if (variant.Index == eventIndex)
                        {
                            eventTypeFields = variant.TypeFields;
                            break;
                        }
                    }

                    return new ExtrinsicEvent(palletName, eventName, GetParametersList(parameters, eventTypeFields ?? []));
                })
            };
        }

        /// <summary>
        /// Gets extrinsic details without events
        /// </summary>
        /// <returns>all events for the given extrinsic</returns>
        public static async Task<ExtrinsicDetails> GetExtrinsicDetailsAsync(
            this SubstrateClientExt substrateClient,
            uint? extrinsicIndex,
            string? _eventsBytes,
            BigInteger blockNumber,
            CancellationToken token = default
        )
        {
            return new ExtrinsicDetails
            {
                BlockNumber = blockNumber,
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

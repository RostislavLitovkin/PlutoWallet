using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Model.Temp;
using PlutoWallet.Types;
using Substrate.NetApi;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

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
        public required string Name { get; set; }
        public required string Value { get; set; }
        public required byte[] EncodedValue { get; set; }
    }
    public static class EventsModel
    {
        public static List<EventParameter> GetParametersList(object? parameters, TypeField[] eventTypeFields)
        {
            if (parameters == null)
            {
                return new List<EventParameter>();
            }

            var parametersList = new List<EventParameter>();

            var pValues = eventTypeFields.Length switch
            {
                0 => [],
                1 => [(IType)parameters],
                _ => (IType[])parameters.GetProperty("Value")
            };

            for (int i = 0; i < pValues.Length; i++)
            {
                try
                {
                    var parameter = pValues[i];
                    var eventTypeField = eventTypeFields[i];

                    Type type = parameter.GetType();

                    var eventParameter = type.Name switch
                    {
                        "AccountId32" => new EventParameter
                        {
                            Name = eventTypeField.Name,
                            Value = Utils.GetAddressFrom(((IType)parameter.GetProperty("Value")).Encode()),
                            EncodedValue = parameter.Encode(),
                        },
                        "Arr32U8" => new EventParameter
                        {
                            Name = eventTypeField.Name,
                            Value = Utils.Bytes2HexString(parameter.Encode()),
                            EncodedValue = parameter.Encode(),
                        },
                        "H256" => new EventParameter
                        {
                            Name = eventTypeField.Name,
                            Value = Utils.Bytes2HexString(((IType)parameter.GetProperty("Value")).Encode()),
                            EncodedValue = parameter.Encode(),
                        },
                        _ => new EventParameter
                        {
                            Name = eventTypeField.Name,
                            Value = parameter.ToString(),
                            EncodedValue = parameter.Encode(),
                        }
                    };

                    parametersList.Add(eventParameter);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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

            Console.WriteLine("block hash: " + blockHashString);

            var eventsParameters = RequestGenerator.GetStorage("System", "Events", Substrate.NetApi.Model.Meta.Storage.Type.Plain);

            string eventsBytes = await substrateClient.SubstrateClient.InvokeAsync<string>("state_getStorage", new object[2] { eventsParameters, blockHashString }, token);

            Console.WriteLine("Events bytes: " + eventsBytes);

            Console.WriteLine("check metadata: " + substrateClient.CheckMetadata);

            if (substrateClient.CheckMetadata)
            {
                BlockData block = await substrateClient.SubstrateClient.Chain.GetBlockAsync(blockHash, CancellationToken.None);

                var sblock = await substrateClient.SubstrateClient.InvokeAsync<object>("chain_getBlock", new object[1] { (string)blockHash.Value }, token);
                Console.WriteLine(sblock);

                Console.WriteLine("block number: " + block.Block.Header.Number.Value);

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

                Console.WriteLine("Extrinsic index found: " + extrinsicIndex);

                return await GetExtrinsicEventsForClientAsync(substrateClient, extrinsicIndex, eventsBytes, blockNumber: block.Block.Header.Number.Value, token);

            }
            else
            {
                var block = await substrateClient.SubstrateClient.InvokeAsync<TempOldBlockData>("chain_getBlock", new object[1] { (string)blockHash.Value }, token);

                Console.WriteLine("block number: " + block.Block.Header.Number.Value);

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

                Console.WriteLine("Extrinsic index found: " + extrinsicIndex);

                return await GetExtrinsicEventsForClientAsync(substrateClient, extrinsicIndex, eventsBytes, blockNumber: block.Block.Header.Number.Value, token);
            }
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
        /// 
        /// If extrinsicIndex is null, it will return all events in the block
        /// </summary>
        /// <returns>all events for the given extrinsic</returns>
        public static async Task<ExtrinsicDetails> GetExtrinsicEventsAsync<T>(
            this SubstrateClientExt substrateClient,
            uint? extrinsicIndex,
            string? eventsBytes,
            BigInteger blockNumber,
            CancellationToken token
        ) where T : BaseType, new()
        {
            if (eventsBytes == null || eventsBytes.Length == 0)
            {

                Console.WriteLine("Something was null in events");
                Console.WriteLine(extrinsicIndex is null);
                Console.WriteLine(eventsBytes);
                Console.WriteLine(eventsBytes.Length);

                return new ExtrinsicDetails
                {
                    BlockNumber = blockNumber,
                    ExtrinsicIndex = extrinsicIndex,
                    Events = new List<ExtrinsicEvent>()
                };
            }

            var events = new BaseVec<UniversalEventRecord<T>>();
            events.Create(eventsBytes);

            var sortedEvents = events.Value.Where(p => extrinsicIndex is null || (p.Phase.Value == PolkadotAssetHub.NetApi.Generated.Model.frame_system.Phase.ApplyExtrinsic && ((U32)p.Phase.Value2).Value == extrinsicIndex));

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

    public sealed class UniversalEventRecord<T> : BaseType where T : BaseType, new()
    {
        /// <summary>
        /// >> phase
        /// </summary>
        private PolkadotAssetHub.NetApi.Generated.Model.frame_system.EnumPhase _phase;

        /// <summary>
        /// >> event
        /// </summary>
        private T _event;

        /// <summary>
        /// >> topics
        /// </summary>
        private Substrate.NetApi.Model.Types.Base.BaseVec<PolkadotAssetHub.NetApi.Generated.Model.primitive_types.H256> _topics;

        public PolkadotAssetHub.NetApi.Generated.Model.frame_system.EnumPhase Phase
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

        public BaseVec<PolkadotAssetHub.NetApi.Generated.Model.primitive_types.H256> Topics
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
            Phase = new PolkadotAssetHub.NetApi.Generated.Model.frame_system.EnumPhase();
            Phase.Decode(byteArray, ref p);
            Event = new T();
            Event.Decode(byteArray, ref p);
            Topics = new Substrate.NetApi.Model.Types.Base.BaseVec<PolkadotAssetHub.NetApi.Generated.Model.primitive_types.H256>();
            Topics.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}

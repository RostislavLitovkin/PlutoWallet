//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;


namespace Substrate.NetApi.Generated.Model.pallet_message_queue.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> ProcessingFailed
        /// Message discarded due to an error in the `MessageProcessor` (usually a format error).
        /// </summary>
        ProcessingFailed = 0,
        
        /// <summary>
        /// >> Processed
        /// Message is processed.
        /// </summary>
        Processed = 1,
        
        /// <summary>
        /// >> OverweightEnqueued
        /// Message placed in overweight queue.
        /// </summary>
        OverweightEnqueued = 2,
        
        /// <summary>
        /// >> PageReaped
        /// This page was reaped.
        /// </summary>
        PageReaped = 3,
    }
    
    /// <summary>
    /// >> 517 - Variant[pallet_message_queue.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumExt<Event, BaseTuple<Substrate.NetApi.Generated.Model.primitive_types.H256, Substrate.NetApi.Generated.Model.polkadot_runtime_parachains.inclusion.EnumAggregateMessageOrigin, Substrate.NetApi.Generated.Model.frame_support.traits.messages.EnumProcessMessageError>, BaseTuple<Substrate.NetApi.Generated.Model.primitive_types.H256, Substrate.NetApi.Generated.Model.polkadot_runtime_parachains.inclusion.EnumAggregateMessageOrigin, Substrate.NetApi.Generated.Model.sp_weights.weight_v2.Weight, Substrate.NetApi.Model.Types.Primitive.Bool>, BaseTuple<Substrate.NetApi.Generated.Types.Base.Arr32U8, Substrate.NetApi.Generated.Model.polkadot_runtime_parachains.inclusion.EnumAggregateMessageOrigin, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>, BaseTuple<Substrate.NetApi.Generated.Model.polkadot_runtime_parachains.inclusion.EnumAggregateMessageOrigin, Substrate.NetApi.Model.Types.Primitive.U32>>
    {
    }
}
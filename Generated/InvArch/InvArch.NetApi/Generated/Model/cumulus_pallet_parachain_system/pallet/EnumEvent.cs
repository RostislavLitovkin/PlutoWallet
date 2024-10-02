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


namespace InvArch.NetApi.Generated.Model.cumulus_pallet_parachain_system.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// 
    ///			The [event](https://docs.substrate.io/main-docs/build/events-errors/) emitted
    ///			by this pallet.
    ///			
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> ValidationFunctionStored
        /// The validation function has been scheduled to apply.
        /// </summary>
        ValidationFunctionStored = 0,
        
        /// <summary>
        /// >> ValidationFunctionApplied
        /// The validation function was applied as of the contained relay chain block number.
        /// </summary>
        ValidationFunctionApplied = 1,
        
        /// <summary>
        /// >> ValidationFunctionDiscarded
        /// The relay-chain aborted the upgrade process.
        /// </summary>
        ValidationFunctionDiscarded = 2,
        
        /// <summary>
        /// >> UpgradeAuthorized
        /// An upgrade has been authorized.
        /// </summary>
        UpgradeAuthorized = 3,
        
        /// <summary>
        /// >> DownwardMessagesReceived
        /// Some downward messages have been received and will be processed.
        /// </summary>
        DownwardMessagesReceived = 4,
        
        /// <summary>
        /// >> DownwardMessagesProcessed
        /// Downward messages were processed using the given weight.
        /// </summary>
        DownwardMessagesProcessed = 5,
        
        /// <summary>
        /// >> UpwardMessageSent
        /// An upward message was sent to the relay chain.
        /// </summary>
        UpwardMessageSent = 6,
    }
    
    /// <summary>
    /// >> 30 - Variant[cumulus_pallet_parachain_system.pallet.Event]
    /// 
    ///			The [event](https://docs.substrate.io/main-docs/build/events-errors/) emitted
    ///			by this pallet.
    ///			
    /// </summary>
    public sealed class EnumEvent : BaseEnumRust<Event>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumEvent()
        {
				AddTypeDecoder<BaseVoid>(Event.ValidationFunctionStored);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Primitive.U32>(Event.ValidationFunctionApplied);
				AddTypeDecoder<BaseVoid>(Event.ValidationFunctionDiscarded);
				AddTypeDecoder<InvArch.NetApi.Generated.Model.primitive_types.H256>(Event.UpgradeAuthorized);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Primitive.U32>(Event.DownwardMessagesReceived);
				AddTypeDecoder<BaseTuple<InvArch.NetApi.Generated.Model.sp_weights.weight_v2.Weight, InvArch.NetApi.Generated.Model.primitive_types.H256>>(Event.DownwardMessagesProcessed);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Base.BaseOpt<InvArch.NetApi.Generated.Types.Base.Arr32U8>>(Event.UpwardMessageSent);
        }
    }
}
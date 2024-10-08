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


namespace Bifrost.NetApi.Generated.Model.bifrost_system_maker.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> Charged
        /// </summary>
        Charged = 0,
        
        /// <summary>
        /// >> ConfigSet
        /// </summary>
        ConfigSet = 1,
        
        /// <summary>
        /// >> Closed
        /// </summary>
        Closed = 2,
        
        /// <summary>
        /// >> Paid
        /// </summary>
        Paid = 3,
        
        /// <summary>
        /// >> RedeemFailed
        /// </summary>
        RedeemFailed = 4,
    }
    
    /// <summary>
    /// >> 515 - Variant[bifrost_system_maker.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumRust<Event>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumEvent()
        {
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.sp_core.crypto.AccountId32, Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Primitive.U128>>(Event.Charged);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Bifrost.NetApi.Generated.Model.bifrost_system_maker.pallet.Info>>(Event.ConfigSet);
				AddTypeDecoder<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId>(Event.Closed);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Primitive.U128>>(Event.Paid);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Primitive.U128>>(Event.RedeemFailed);
        }
    }
}

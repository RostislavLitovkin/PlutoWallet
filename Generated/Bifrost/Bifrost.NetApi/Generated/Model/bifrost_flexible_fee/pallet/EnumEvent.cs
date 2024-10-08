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


namespace Bifrost.NetApi.Generated.Model.bifrost_flexible_fee.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> TransferTo
        /// </summary>
        TransferTo = 0,
        
        /// <summary>
        /// >> FlexibleFeeExchanged
        /// </summary>
        FlexibleFeeExchanged = 1,
        
        /// <summary>
        /// >> FixedRateFeeExchanged
        /// </summary>
        FixedRateFeeExchanged = 2,
        
        /// <summary>
        /// >> ExtraFeeDeducted
        /// </summary>
        ExtraFeeDeducted = 3,
    }
    
    /// <summary>
    /// >> 493 - Variant[bifrost_flexible_fee.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumRust<Event>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumEvent()
        {
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.sp_core.crypto.AccountId32, Bifrost.NetApi.Generated.Model.bifrost_flexible_fee.EnumTargetChain, Substrate.NetApi.Model.Types.Primitive.U128>>(Event.TransferTo);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Primitive.U128>>(Event.FlexibleFeeExchanged);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Primitive.U128>>(Event.FixedRateFeeExchanged);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.EnumExtraFeeName, Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Primitive.U128, Substrate.NetApi.Model.Types.Primitive.U128, Bifrost.NetApi.Generated.Model.sp_core.crypto.AccountId32>>(Event.ExtraFeeDeducted);
        }
    }
}

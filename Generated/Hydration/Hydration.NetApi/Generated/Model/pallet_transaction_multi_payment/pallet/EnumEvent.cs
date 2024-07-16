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


namespace Hydration.NetApi.Generated.Model.pallet_transaction_multi_payment.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> CurrencySet
        /// CurrencySet
        /// [who, currency]
        /// </summary>
        CurrencySet = 0,
        
        /// <summary>
        /// >> CurrencyAdded
        /// New accepted currency added
        /// [currency]
        /// </summary>
        CurrencyAdded = 1,
        
        /// <summary>
        /// >> CurrencyRemoved
        /// Accepted currency removed
        /// [currency]
        /// </summary>
        CurrencyRemoved = 2,
        
        /// <summary>
        /// >> FeeWithdrawn
        /// Transaction fee paid in non-native currency
        /// [Account, Currency, Native fee amount, Non-native fee amount, Destination account]
        /// </summary>
        FeeWithdrawn = 3,
    }
    
    /// <summary>
    /// >> 34 - Variant[pallet_transaction_multi_payment.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumExt<Event, BaseTuple<Hydration.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, BaseTuple<Hydration.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U128, Substrate.NetApi.Model.Types.Primitive.U128, Hydration.NetApi.Generated.Model.sp_core.crypto.AccountId32>>
    {
    }
}
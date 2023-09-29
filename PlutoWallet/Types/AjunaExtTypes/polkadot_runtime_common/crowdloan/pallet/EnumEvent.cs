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


namespace Substrate.NetApi.Generated.Model.polkadot_runtime_common.crowdloan.pallet
{
    
    
    public enum Event
    {
        
        Created = 0,
        
        Contributed = 1,
        
        Withdrew = 2,
        
        PartiallyRefunded = 3,
        
        AllRefunded = 4,
        
        Dissolved = 5,
        
        HandleBidResult = 6,
        
        Edited = 7,
        
        MemoUpdated = 8,
        
        AddedToNewRaise = 9,
    }
    
    /// <summary>
    /// >> 120 - Variant[polkadot_runtime_common.crowdloan.pallet.Event]
    /// 
    ///			The [event](https://docs.substrate.io/main-docs/build/events-errors/) emitted
    ///			by this pallet.
    ///			
    /// </summary>
    public sealed class EnumEvent : BaseEnumExt<Event, Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id, BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id, Substrate.NetApi.Model.Types.Primitive.U128>, BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id, Substrate.NetApi.Model.Types.Primitive.U128>, Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id, Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id, Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id, BaseTuple<Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id, Substrate.NetApi.Generated.Types.Base.EnumResult>, Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id, BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>>, Substrate.NetApi.Generated.Model.polkadot_parachain.primitives.Id>
    {
    }
}

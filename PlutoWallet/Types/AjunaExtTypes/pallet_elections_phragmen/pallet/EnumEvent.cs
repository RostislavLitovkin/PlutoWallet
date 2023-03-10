//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ajuna.NetApi.Model.Types.Base;
using System.Collections.Generic;


namespace PlutoWallet.NetApiExt.Generated.Model.pallet_elections_phragmen.pallet
{
    
    
    public enum Event
    {
        
        NewTerm = 0,
        
        EmptyTerm = 1,
        
        ElectionError = 2,
        
        MemberKicked = 3,
        
        Renounced = 4,
        
        CandidateSlashed = 5,
        
        SeatHolderSlashed = 6,
    }
    
    /// <summary>
    /// >> 67 - Variant[pallet_elections_phragmen.pallet.Event]
    /// 
    ///			The [event](https://docs.substrate.io/main-docs/build/events-errors/) emitted
    ///			by this pallet.
    ///			
    /// </summary>
    public sealed class EnumEvent : BaseEnumExt<Event, Ajuna.NetApi.Model.Types.Base.BaseVec<Ajuna.NetApi.Model.Types.Base.BaseTuple<PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Ajuna.NetApi.Model.Types.Primitive.U128>>, BaseVoid, BaseVoid, PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, BaseTuple<PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Ajuna.NetApi.Model.Types.Primitive.U128>, BaseTuple<PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Ajuna.NetApi.Model.Types.Primitive.U128>>
    {
    }
}

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


namespace Substrate.NetApi.Generated.Model.xcm.v2.junction
{
    
    
    public enum Junction
    {
        
        Parachain = 0,
        
        AccountId32 = 1,
        
        AccountIndex64 = 2,
        
        AccountKey20 = 3,
        
        PalletInstance = 4,
        
        GeneralIndex = 5,
        
        GeneralKey = 6,
        
        OnlyChild = 7,
        
        Plurality = 8,
    }
    
    /// <summary>
    /// >> 91 - Variant[xcm.v2.junction.Junction]
    /// </summary>
    public sealed class EnumJunction : BaseEnumExt<Junction, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v2.EnumNetworkId, Substrate.NetApi.Generated.Types.Base.Arr32U8>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v2.EnumNetworkId, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U64>>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v2.EnumNetworkId, Substrate.NetApi.Generated.Types.Base.Arr20U8>, Substrate.NetApi.Model.Types.Primitive.U8, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128>, Substrate.NetApi.Generated.Model.bounded_collections.weak_bounded_vec.WeakBoundedVecT1, BaseVoid, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v2.EnumBodyId, Substrate.NetApi.Generated.Model.xcm.v2.EnumBodyPart>>
    {
    }
}

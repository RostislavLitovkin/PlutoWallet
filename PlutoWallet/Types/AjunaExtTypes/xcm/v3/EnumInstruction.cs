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


namespace Substrate.NetApi.Generated.Model.xcm.v3
{
    
    
    public enum Instruction
    {
        
        WithdrawAsset = 0,
        
        ReserveAssetDeposited = 1,
        
        ReceiveTeleportedAsset = 2,
        
        QueryResponse = 3,
        
        TransferAsset = 4,
        
        TransferReserveAsset = 5,
        
        Transact = 6,
        
        HrmpNewChannelOpenRequest = 7,
        
        HrmpChannelAccepted = 8,
        
        HrmpChannelClosing = 9,
        
        ClearOrigin = 10,
        
        DescendOrigin = 11,
        
        ReportError = 12,
        
        DepositAsset = 13,
        
        DepositReserveAsset = 14,
        
        ExchangeAsset = 15,
        
        InitiateReserveWithdraw = 16,
        
        InitiateTeleport = 17,
        
        ReportHolding = 18,
        
        BuyExecution = 19,
        
        RefundSurplus = 20,
        
        SetErrorHandler = 21,
        
        SetAppendix = 22,
        
        ClearError = 23,
        
        ClaimAsset = 24,
        
        Trap = 25,
        
        SubscribeVersion = 26,
        
        UnsubscribeVersion = 27,
        
        BurnAsset = 28,
        
        ExpectAsset = 29,
        
        ExpectOrigin = 30,
        
        ExpectError = 31,
        
        ExpectTransactStatus = 32,
        
        QueryPallet = 33,
        
        ExpectPallet = 34,
        
        ReportTransactStatus = 35,
        
        ClearTransactStatus = 36,
        
        UniversalOrigin = 37,
        
        ExportMessage = 38,
        
        LockAsset = 39,
        
        UnlockAsset = 40,
        
        NoteUnlockable = 41,
        
        RequestUnlock = 42,
        
        SetFeesMode = 43,
        
        SetTopic = 44,
        
        ClearTopic = 45,
        
        AliasOrigin = 46,
        
        UnpaidExecution = 47,
    }
    
    /// <summary>
    /// >> 263 - Variant[xcm.v3.Instruction]
    /// </summary>
    public sealed class EnumInstruction : BaseEnumExt<Instruction, Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAssets, Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAssets, Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAssets, BaseTuple<Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U64>, Substrate.NetApi.Generated.Model.xcm.v3.EnumResponse, Substrate.NetApi.Generated.Model.sp_weights.weight_v2.Weight, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAssets, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAssets, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation, Substrate.NetApi.Generated.Model.xcm.v3.XcmT1>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v2.EnumOriginKind, Substrate.NetApi.Generated.Model.sp_weights.weight_v2.Weight, Substrate.NetApi.Generated.Model.xcm.double_encoded.DoubleEncodedT2>, BaseTuple<Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, BaseTuple<Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>>, BaseVoid, Substrate.NetApi.Generated.Model.xcm.v3.junctions.EnumJunctions, Substrate.NetApi.Generated.Model.xcm.v3.QueryResponseInfo, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.EnumMultiAssetFilter, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.EnumMultiAssetFilter, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation, Substrate.NetApi.Generated.Model.xcm.v3.XcmT1>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.EnumMultiAssetFilter, Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAssets, Substrate.NetApi.Model.Types.Primitive.Bool>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.EnumMultiAssetFilter, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation, Substrate.NetApi.Generated.Model.xcm.v3.XcmT1>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.EnumMultiAssetFilter, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation, Substrate.NetApi.Generated.Model.xcm.v3.XcmT1>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.QueryResponseInfo, Substrate.NetApi.Generated.Model.xcm.v3.multiasset.EnumMultiAssetFilter>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAsset, Substrate.NetApi.Generated.Model.xcm.v3.EnumWeightLimit>, BaseVoid, Substrate.NetApi.Generated.Model.xcm.v3.XcmT2, Substrate.NetApi.Generated.Model.xcm.v3.XcmT2, BaseVoid, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAssets, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U64>, BaseTuple<Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U64>, Substrate.NetApi.Generated.Model.sp_weights.weight_v2.Weight>, BaseVoid, Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAssets, Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAssets, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Generated.Model.xcm.v3.traits.EnumError>>, Substrate.NetApi.Generated.Model.xcm.v3.EnumMaybeErrorCode, BaseTuple<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>, Substrate.NetApi.Generated.Model.xcm.v3.QueryResponseInfo>, BaseTuple<Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>>, Substrate.NetApi.Generated.Model.xcm.v3.QueryResponseInfo, BaseVoid, Substrate.NetApi.Generated.Model.xcm.v3.junction.EnumJunction, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.junction.EnumNetworkId, Substrate.NetApi.Generated.Model.xcm.v3.junctions.EnumJunctions, Substrate.NetApi.Generated.Model.xcm.v3.XcmT1>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAsset, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAsset, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAsset, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.multiasset.MultiAsset, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>, Substrate.NetApi.Model.Types.Primitive.Bool, Substrate.NetApi.Generated.Types.Base.Arr32U8, BaseVoid, Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation, BaseTuple<Substrate.NetApi.Generated.Model.xcm.v3.EnumWeightLimit, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>>>
    {
    }
}

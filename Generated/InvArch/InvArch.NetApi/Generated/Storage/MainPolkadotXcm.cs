//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace InvArch.NetApi.Generated.Storage
{
    
    
    /// <summary>
    /// >> PolkadotXcmStorage
    /// </summary>
    public sealed class PolkadotXcmStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> PolkadotXcmStorage Constructor
        /// </summary>
        public PolkadotXcmStorage(SubstrateClientExt client)
        {
            this._client = client;
        }
    }
    
    /// <summary>
    /// >> PolkadotXcmCalls
    /// </summary>
    public sealed class PolkadotXcmCalls
    {
        
        /// <summary>
        /// >> send
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Send(InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation dest, InvArch.NetApi.Generated.Model.xcm.EnumVersionedXcm message)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(message.Encode());
            return new Method(31, "PolkadotXcm", 0, "send", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> teleport_assets
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method TeleportAssets(InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation dest, InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation beneficiary, InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiAssets assets, Substrate.NetApi.Model.Types.Primitive.U32 fee_asset_item)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(beneficiary.Encode());
            byteArray.AddRange(assets.Encode());
            byteArray.AddRange(fee_asset_item.Encode());
            return new Method(31, "PolkadotXcm", 1, "teleport_assets", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> reserve_transfer_assets
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method ReserveTransferAssets(InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation dest, InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation beneficiary, InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiAssets assets, Substrate.NetApi.Model.Types.Primitive.U32 fee_asset_item)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(beneficiary.Encode());
            byteArray.AddRange(assets.Encode());
            byteArray.AddRange(fee_asset_item.Encode());
            return new Method(31, "PolkadotXcm", 2, "reserve_transfer_assets", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> execute
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Execute(InvArch.NetApi.Generated.Model.xcm.EnumVersionedXcm message, InvArch.NetApi.Generated.Model.sp_weights.weight_v2.Weight max_weight)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(message.Encode());
            byteArray.AddRange(max_weight.Encode());
            return new Method(31, "PolkadotXcm", 3, "execute", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> force_xcm_version
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method ForceXcmVersion(InvArch.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation location, Substrate.NetApi.Model.Types.Primitive.U32 xcm_version)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(location.Encode());
            byteArray.AddRange(xcm_version.Encode());
            return new Method(31, "PolkadotXcm", 4, "force_xcm_version", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> force_default_xcm_version
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method ForceDefaultXcmVersion(Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U32> maybe_xcm_version)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(maybe_xcm_version.Encode());
            return new Method(31, "PolkadotXcm", 5, "force_default_xcm_version", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> force_subscribe_version_notify
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method ForceSubscribeVersionNotify(InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation location)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(location.Encode());
            return new Method(31, "PolkadotXcm", 6, "force_subscribe_version_notify", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> force_unsubscribe_version_notify
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method ForceUnsubscribeVersionNotify(InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation location)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(location.Encode());
            return new Method(31, "PolkadotXcm", 7, "force_unsubscribe_version_notify", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> limited_reserve_transfer_assets
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method LimitedReserveTransferAssets(InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation dest, InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation beneficiary, InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiAssets assets, Substrate.NetApi.Model.Types.Primitive.U32 fee_asset_item, InvArch.NetApi.Generated.Model.xcm.v3.EnumWeightLimit weight_limit)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(beneficiary.Encode());
            byteArray.AddRange(assets.Encode());
            byteArray.AddRange(fee_asset_item.Encode());
            byteArray.AddRange(weight_limit.Encode());
            return new Method(31, "PolkadotXcm", 8, "limited_reserve_transfer_assets", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> limited_teleport_assets
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method LimitedTeleportAssets(InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation dest, InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation beneficiary, InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiAssets assets, Substrate.NetApi.Model.Types.Primitive.U32 fee_asset_item, InvArch.NetApi.Generated.Model.xcm.v3.EnumWeightLimit weight_limit)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(beneficiary.Encode());
            byteArray.AddRange(assets.Encode());
            byteArray.AddRange(fee_asset_item.Encode());
            byteArray.AddRange(weight_limit.Encode());
            return new Method(31, "PolkadotXcm", 9, "limited_teleport_assets", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> force_suspension
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method ForceSuspension(Substrate.NetApi.Model.Types.Primitive.Bool suspended)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(suspended.Encode());
            return new Method(31, "PolkadotXcm", 10, "force_suspension", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> PolkadotXcmConstants
    /// </summary>
    public sealed class PolkadotXcmConstants
    {
    }
    
    /// <summary>
    /// >> PolkadotXcmErrors
    /// </summary>
    public enum PolkadotXcmErrors
    {
        
        /// <summary>
        /// >> Unreachable
        /// The desired destination was unreachable, generally because there is a no way of routing
        /// to it.
        /// </summary>
        Unreachable,
        
        /// <summary>
        /// >> SendFailure
        /// There was some other issue (i.e. not to do with routing) in sending the message. Perhaps
        /// a lack of space for buffering the message.
        /// </summary>
        SendFailure,
        
        /// <summary>
        /// >> Filtered
        /// The message execution fails the filter.
        /// </summary>
        Filtered,
        
        /// <summary>
        /// >> UnweighableMessage
        /// The message's weight could not be determined.
        /// </summary>
        UnweighableMessage,
        
        /// <summary>
        /// >> DestinationNotInvertible
        /// The destination `MultiLocation` provided cannot be inverted.
        /// </summary>
        DestinationNotInvertible,
        
        /// <summary>
        /// >> Empty
        /// The assets to be sent are empty.
        /// </summary>
        Empty,
        
        /// <summary>
        /// >> CannotReanchor
        /// Could not re-anchor the assets to declare the fees for the destination chain.
        /// </summary>
        CannotReanchor,
        
        /// <summary>
        /// >> TooManyAssets
        /// Too many assets have been attempted for transfer.
        /// </summary>
        TooManyAssets,
        
        /// <summary>
        /// >> InvalidOrigin
        /// Origin is invalid for sending.
        /// </summary>
        InvalidOrigin,
        
        /// <summary>
        /// >> BadVersion
        /// The version of the `Versioned` value used is not able to be interpreted.
        /// </summary>
        BadVersion,
        
        /// <summary>
        /// >> BadLocation
        /// The given location could not be used (e.g. because it cannot be expressed in the
        /// desired version of XCM).
        /// </summary>
        BadLocation,
        
        /// <summary>
        /// >> NoSubscription
        /// The referenced subscription could not be found.
        /// </summary>
        NoSubscription,
        
        /// <summary>
        /// >> AlreadySubscribed
        /// The location is invalid since it already has a subscription from us.
        /// </summary>
        AlreadySubscribed,
        
        /// <summary>
        /// >> InvalidAsset
        /// Invalid asset for the operation.
        /// </summary>
        InvalidAsset,
        
        /// <summary>
        /// >> LowBalance
        /// The owner does not own (all) of the asset that they wish to do the operation on.
        /// </summary>
        LowBalance,
        
        /// <summary>
        /// >> TooManyLocks
        /// The asset owner has too many locks on the asset.
        /// </summary>
        TooManyLocks,
        
        /// <summary>
        /// >> AccountNotSovereign
        /// The given account is not an identifiable sovereign account for any location.
        /// </summary>
        AccountNotSovereign,
        
        /// <summary>
        /// >> FeesNotMet
        /// The operation required fees to be paid which the initiator could not meet.
        /// </summary>
        FeesNotMet,
        
        /// <summary>
        /// >> LockNotFound
        /// A remote lock with the corresponding data could not be found.
        /// </summary>
        LockNotFound,
        
        /// <summary>
        /// >> InUse
        /// The unlock operation cannot succeed because there are still consumers of the lock.
        /// </summary>
        InUse,
    }
}

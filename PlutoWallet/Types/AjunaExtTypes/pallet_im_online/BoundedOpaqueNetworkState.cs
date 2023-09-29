//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Attributes;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Metadata.V14;
using System.Collections.Generic;


namespace Substrate.NetApi.Generated.Model.pallet_im_online
{
    
    
    /// <summary>
    /// >> 522 - Composite[pallet_im_online.BoundedOpaqueNetworkState]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class BoundedOpaqueNetworkState : BaseType
    {
        
        /// <summary>
        /// >> peer_id
        /// </summary>
        private Substrate.NetApi.Generated.Model.sp_core.bounded.weak_bounded_vec.WeakBoundedVecT6 _peerId;
        
        /// <summary>
        /// >> external_addresses
        /// </summary>
        private Substrate.NetApi.Generated.Model.sp_core.bounded.weak_bounded_vec.WeakBoundedVecT7 _externalAddresses;
        
        public Substrate.NetApi.Generated.Model.sp_core.bounded.weak_bounded_vec.WeakBoundedVecT6 PeerId
        {
            get
            {
                return this._peerId;
            }
            set
            {
                this._peerId = value;
            }
        }
        
        public Substrate.NetApi.Generated.Model.sp_core.bounded.weak_bounded_vec.WeakBoundedVecT7 ExternalAddresses
        {
            get
            {
                return this._externalAddresses;
            }
            set
            {
                this._externalAddresses = value;
            }
        }
        
        public override string TypeName()
        {
            return "BoundedOpaqueNetworkState";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(PeerId.Encode());
            result.AddRange(ExternalAddresses.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            PeerId = new Substrate.NetApi.Generated.Model.sp_core.bounded.weak_bounded_vec.WeakBoundedVecT6();
            PeerId.Decode(byteArray, ref p);
            ExternalAddresses = new Substrate.NetApi.Generated.Model.sp_core.bounded.weak_bounded_vec.WeakBoundedVecT7();
            ExternalAddresses.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}

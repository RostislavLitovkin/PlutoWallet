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
    /// >> 228 - Composite[pallet_im_online.Heartbeat]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class Heartbeat : BaseType
    {
        
        /// <summary>
        /// >> block_number
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _blockNumber;
        
        /// <summary>
        /// >> network_state
        /// </summary>
        private Substrate.NetApi.Generated.Model.sp_core.offchain.OpaqueNetworkState _networkState;
        
        /// <summary>
        /// >> session_index
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _sessionIndex;
        
        /// <summary>
        /// >> authority_index
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _authorityIndex;
        
        /// <summary>
        /// >> validators_len
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _validatorsLen;
        
        public Substrate.NetApi.Model.Types.Primitive.U32 BlockNumber
        {
            get
            {
                return this._blockNumber;
            }
            set
            {
                this._blockNumber = value;
            }
        }
        
        public Substrate.NetApi.Generated.Model.sp_core.offchain.OpaqueNetworkState NetworkState
        {
            get
            {
                return this._networkState;
            }
            set
            {
                this._networkState = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Primitive.U32 SessionIndex
        {
            get
            {
                return this._sessionIndex;
            }
            set
            {
                this._sessionIndex = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Primitive.U32 AuthorityIndex
        {
            get
            {
                return this._authorityIndex;
            }
            set
            {
                this._authorityIndex = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Primitive.U32 ValidatorsLen
        {
            get
            {
                return this._validatorsLen;
            }
            set
            {
                this._validatorsLen = value;
            }
        }
        
        public override string TypeName()
        {
            return "Heartbeat";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(BlockNumber.Encode());
            result.AddRange(NetworkState.Encode());
            result.AddRange(SessionIndex.Encode());
            result.AddRange(AuthorityIndex.Encode());
            result.AddRange(ValidatorsLen.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            BlockNumber = new Substrate.NetApi.Model.Types.Primitive.U32();
            BlockNumber.Decode(byteArray, ref p);
            NetworkState = new Substrate.NetApi.Generated.Model.sp_core.offchain.OpaqueNetworkState();
            NetworkState.Decode(byteArray, ref p);
            SessionIndex = new Substrate.NetApi.Model.Types.Primitive.U32();
            SessionIndex.Decode(byteArray, ref p);
            AuthorityIndex = new Substrate.NetApi.Model.Types.Primitive.U32();
            AuthorityIndex.Decode(byteArray, ref p);
            ValidatorsLen = new Substrate.NetApi.Model.Types.Primitive.U32();
            ValidatorsLen.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}

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


namespace Substrate.NetApi.Generated.Model.polkadot_runtime_parachains.hrmp
{
    
    
    /// <summary>
    /// >> 686 - Composite[polkadot_runtime_parachains.hrmp.HrmpOpenChannelRequest]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class HrmpOpenChannelRequest : BaseType
    {
        
        /// <summary>
        /// >> confirmed
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.Bool _confirmed;
        
        /// <summary>
        /// >> _age
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _age;
        
        /// <summary>
        /// >> sender_deposit
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U128 _senderDeposit;
        
        /// <summary>
        /// >> max_message_size
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _maxMessageSize;
        
        /// <summary>
        /// >> max_capacity
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _maxCapacity;
        
        /// <summary>
        /// >> max_total_size
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _maxTotalSize;
        
        public Substrate.NetApi.Model.Types.Primitive.Bool Confirmed
        {
            get
            {
                return this._confirmed;
            }
            set
            {
                this._confirmed = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Primitive.U32 Age
        {
            get
            {
                return this._age;
            }
            set
            {
                this._age = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Primitive.U128 SenderDeposit
        {
            get
            {
                return this._senderDeposit;
            }
            set
            {
                this._senderDeposit = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxMessageSize
        {
            get
            {
                return this._maxMessageSize;
            }
            set
            {
                this._maxMessageSize = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxCapacity
        {
            get
            {
                return this._maxCapacity;
            }
            set
            {
                this._maxCapacity = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxTotalSize
        {
            get
            {
                return this._maxTotalSize;
            }
            set
            {
                this._maxTotalSize = value;
            }
        }
        
        public override string TypeName()
        {
            return "HrmpOpenChannelRequest";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Confirmed.Encode());
            result.AddRange(Age.Encode());
            result.AddRange(SenderDeposit.Encode());
            result.AddRange(MaxMessageSize.Encode());
            result.AddRange(MaxCapacity.Encode());
            result.AddRange(MaxTotalSize.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Confirmed = new Substrate.NetApi.Model.Types.Primitive.Bool();
            Confirmed.Decode(byteArray, ref p);
            Age = new Substrate.NetApi.Model.Types.Primitive.U32();
            Age.Decode(byteArray, ref p);
            SenderDeposit = new Substrate.NetApi.Model.Types.Primitive.U128();
            SenderDeposit.Decode(byteArray, ref p);
            MaxMessageSize = new Substrate.NetApi.Model.Types.Primitive.U32();
            MaxMessageSize.Decode(byteArray, ref p);
            MaxCapacity = new Substrate.NetApi.Model.Types.Primitive.U32();
            MaxCapacity.Decode(byteArray, ref p);
            MaxTotalSize = new Substrate.NetApi.Model.Types.Primitive.U32();
            MaxTotalSize.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}

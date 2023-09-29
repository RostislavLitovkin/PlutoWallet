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
    /// >> 688 - Composite[polkadot_runtime_parachains.hrmp.HrmpChannel]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class HrmpChannel : BaseType
    {
        
        /// <summary>
        /// >> max_capacity
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _maxCapacity;
        
        /// <summary>
        /// >> max_total_size
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _maxTotalSize;
        
        /// <summary>
        /// >> max_message_size
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _maxMessageSize;
        
        /// <summary>
        /// >> msg_count
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _msgCount;
        
        /// <summary>
        /// >> total_size
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _totalSize;
        
        /// <summary>
        /// >> mqc_head
        /// </summary>
        private Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Generated.Model.primitive_types.H256> _mqcHead;
        
        /// <summary>
        /// >> sender_deposit
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U128 _senderDeposit;
        
        /// <summary>
        /// >> recipient_deposit
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U128 _recipientDeposit;
        
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
        
        public Substrate.NetApi.Model.Types.Primitive.U32 MsgCount
        {
            get
            {
                return this._msgCount;
            }
            set
            {
                this._msgCount = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Primitive.U32 TotalSize
        {
            get
            {
                return this._totalSize;
            }
            set
            {
                this._totalSize = value;
            }
        }
        
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Generated.Model.primitive_types.H256> MqcHead
        {
            get
            {
                return this._mqcHead;
            }
            set
            {
                this._mqcHead = value;
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
        
        public Substrate.NetApi.Model.Types.Primitive.U128 RecipientDeposit
        {
            get
            {
                return this._recipientDeposit;
            }
            set
            {
                this._recipientDeposit = value;
            }
        }
        
        public override string TypeName()
        {
            return "HrmpChannel";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(MaxCapacity.Encode());
            result.AddRange(MaxTotalSize.Encode());
            result.AddRange(MaxMessageSize.Encode());
            result.AddRange(MsgCount.Encode());
            result.AddRange(TotalSize.Encode());
            result.AddRange(MqcHead.Encode());
            result.AddRange(SenderDeposit.Encode());
            result.AddRange(RecipientDeposit.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            MaxCapacity = new Substrate.NetApi.Model.Types.Primitive.U32();
            MaxCapacity.Decode(byteArray, ref p);
            MaxTotalSize = new Substrate.NetApi.Model.Types.Primitive.U32();
            MaxTotalSize.Decode(byteArray, ref p);
            MaxMessageSize = new Substrate.NetApi.Model.Types.Primitive.U32();
            MaxMessageSize.Decode(byteArray, ref p);
            MsgCount = new Substrate.NetApi.Model.Types.Primitive.U32();
            MsgCount.Decode(byteArray, ref p);
            TotalSize = new Substrate.NetApi.Model.Types.Primitive.U32();
            TotalSize.Decode(byteArray, ref p);
            MqcHead = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Generated.Model.primitive_types.H256>();
            MqcHead.Decode(byteArray, ref p);
            SenderDeposit = new Substrate.NetApi.Model.Types.Primitive.U128();
            SenderDeposit.Decode(byteArray, ref p);
            RecipientDeposit = new Substrate.NetApi.Model.Types.Primitive.U128();
            RecipientDeposit.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}

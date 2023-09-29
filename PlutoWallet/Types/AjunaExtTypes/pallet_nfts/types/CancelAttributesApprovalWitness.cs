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


namespace Substrate.NetApi.Generated.Model.pallet_nfts.types
{
    
    
    /// <summary>
    /// >> 296 - Composite[pallet_nfts.types.CancelAttributesApprovalWitness]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class CancelAttributesApprovalWitness : BaseType
    {
        
        /// <summary>
        /// >> account_attributes
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _accountAttributes;
        
        public Substrate.NetApi.Model.Types.Primitive.U32 AccountAttributes
        {
            get
            {
                return this._accountAttributes;
            }
            set
            {
                this._accountAttributes = value;
            }
        }
        
        public override string TypeName()
        {
            return "CancelAttributesApprovalWitness";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(AccountAttributes.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            AccountAttributes = new Substrate.NetApi.Model.Types.Primitive.U32();
            AccountAttributes.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}

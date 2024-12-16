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
using Substrate.NetApi.Model.Types.Metadata.Base;
using System.Collections.Generic;


namespace InvArch.NetApi.Generated.Model.pallet_dao_manager.origin
{
    
    
    /// <summary>
    /// >> 191 - Composite[pallet_dao_manager.origin.MultisigInternalOrigin]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class MultisigInternalOrigin : BaseType
    {
        
        /// <summary>
        /// >> id
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 Id { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "MultisigInternalOrigin";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Id.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Id = new Substrate.NetApi.Model.Types.Primitive.U32();
            Id.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
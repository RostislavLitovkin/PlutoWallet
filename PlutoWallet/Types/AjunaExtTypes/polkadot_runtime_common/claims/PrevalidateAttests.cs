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


namespace Substrate.NetApi.Generated.Model.polkadot_runtime_common.claims
{
    
    
    /// <summary>
    /// >> 738 - Composite[polkadot_runtime_common.claims.PrevalidateAttests]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class PrevalidateAttests : BaseType
    {
        
        public override string TypeName()
        {
            return "PrevalidateAttests";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            TypeSize = p - start;
        }
    }
}

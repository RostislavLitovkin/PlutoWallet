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


namespace InvArch.NetApi.Generated.Model.xcm.v3
{
    
    
    /// <summary>
    /// >> 87 - Composite[xcm.v3.QueryResponseInfo]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class QueryResponseInfo : BaseType
    {
        
        /// <summary>
        /// >> destination
        /// </summary>
        public InvArch.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation Destination { get; set; }
        /// <summary>
        /// >> query_id
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U64> QueryId { get; set; }
        /// <summary>
        /// >> max_weight
        /// </summary>
        public InvArch.NetApi.Generated.Model.sp_weights.weight_v2.Weight MaxWeight { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "QueryResponseInfo";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Destination.Encode());
            result.AddRange(QueryId.Encode());
            result.AddRange(MaxWeight.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Destination = new InvArch.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation();
            Destination.Decode(byteArray, ref p);
            QueryId = new Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U64>();
            QueryId.Decode(byteArray, ref p);
            MaxWeight = new InvArch.NetApi.Generated.Model.sp_weights.weight_v2.Weight();
            MaxWeight.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
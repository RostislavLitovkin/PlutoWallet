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


namespace InvArch.NetApi.Generated.Model.pallet_xcm.pallet
{
    
    
    /// <summary>
    /// >> Origin
    /// </summary>
    public enum Origin
    {
        
        /// <summary>
        /// >> Xcm
        /// </summary>
        Xcm = 0,
        
        /// <summary>
        /// >> Response
        /// </summary>
        Response = 1,
    }
    
    /// <summary>
    /// >> 153 - Variant[pallet_xcm.pallet.Origin]
    /// </summary>
    public sealed class EnumOrigin : BaseEnumRust<Origin>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumOrigin()
        {
				AddTypeDecoder<InvArch.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>(Origin.Xcm);
				AddTypeDecoder<InvArch.NetApi.Generated.Model.xcm.v3.multilocation.MultiLocation>(Origin.Response);
        }
    }
}

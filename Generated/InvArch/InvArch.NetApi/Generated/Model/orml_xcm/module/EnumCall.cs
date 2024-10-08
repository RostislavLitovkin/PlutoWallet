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


namespace InvArch.NetApi.Generated.Model.orml_xcm.module
{
    
    
    /// <summary>
    /// >> Call
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public enum Call
    {
        
        /// <summary>
        /// >> send_as_sovereign
        /// Send an XCM message as parachain sovereign.
        /// </summary>
        send_as_sovereign = 0,
    }
    
    /// <summary>
    /// >> 193 - Variant[orml_xcm.module.Call]
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class EnumCall : BaseEnumRust<Call>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumCall()
        {
				AddTypeDecoder<BaseTuple<InvArch.NetApi.Generated.Model.xcm.EnumVersionedMultiLocation, InvArch.NetApi.Generated.Model.xcm.EnumVersionedXcm>>(Call.send_as_sovereign);
        }
    }
}

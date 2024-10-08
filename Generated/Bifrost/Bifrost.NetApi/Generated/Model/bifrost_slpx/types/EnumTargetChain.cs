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


namespace Bifrost.NetApi.Generated.Model.bifrost_slpx.types
{
    
    
    /// <summary>
    /// >> TargetChain
    /// </summary>
    public enum TargetChain
    {
        
        /// <summary>
        /// >> Astar
        /// </summary>
        Astar = 0,
        
        /// <summary>
        /// >> Moonbeam
        /// </summary>
        Moonbeam = 1,
        
        /// <summary>
        /// >> Hydradx
        /// </summary>
        Hydradx = 2,
        
        /// <summary>
        /// >> Interlay
        /// </summary>
        Interlay = 3,
        
        /// <summary>
        /// >> Manta
        /// </summary>
        Manta = 4,
    }
    
    /// <summary>
    /// >> 421 - Variant[bifrost_slpx.types.TargetChain]
    /// </summary>
    public sealed class EnumTargetChain : BaseEnumRust<TargetChain>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumTargetChain()
        {
				AddTypeDecoder<Bifrost.NetApi.Generated.Model.primitive_types.H160>(TargetChain.Astar);
				AddTypeDecoder<Bifrost.NetApi.Generated.Model.primitive_types.H160>(TargetChain.Moonbeam);
				AddTypeDecoder<Bifrost.NetApi.Generated.Model.sp_core.crypto.AccountId32>(TargetChain.Hydradx);
				AddTypeDecoder<Bifrost.NetApi.Generated.Model.sp_core.crypto.AccountId32>(TargetChain.Interlay);
				AddTypeDecoder<Bifrost.NetApi.Generated.Model.sp_core.crypto.AccountId32>(TargetChain.Manta);
        }
    }
}

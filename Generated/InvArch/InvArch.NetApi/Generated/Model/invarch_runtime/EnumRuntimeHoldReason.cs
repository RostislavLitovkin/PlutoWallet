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


namespace InvArch.NetApi.Generated.Model.invarch_runtime
{
    
    
    /// <summary>
    /// >> RuntimeHoldReason
    /// </summary>
    public enum RuntimeHoldReason
    {
        
        /// <summary>
        /// >> Contracts
        /// </summary>
        Contracts = 41,
        
        /// <summary>
        /// >> OcifStaking
        /// </summary>
        OcifStaking = 51,
        
        /// <summary>
        /// >> INV4
        /// </summary>
        INV4 = 71,
    }
    
    /// <summary>
    /// >> 374 - Variant[invarch_runtime.RuntimeHoldReason]
    /// </summary>
    public sealed class EnumRuntimeHoldReason : BaseEnumRust<RuntimeHoldReason>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumRuntimeHoldReason()
        {
				AddTypeDecoder<InvArch.NetApi.Generated.Model.pallet_contracts.pallet.EnumHoldReason>(RuntimeHoldReason.Contracts);
				AddTypeDecoder<InvArch.NetApi.Generated.Model.pallet_dao_staking.pallet.EnumHoldReason>(RuntimeHoldReason.OcifStaking);
				AddTypeDecoder<InvArch.NetApi.Generated.Model.pallet_dao_manager.pallet.EnumHoldReason>(RuntimeHoldReason.INV4);
        }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ajuna.NetApi.Model.Types.Base;
using System.Collections.Generic;


namespace PlutoWallet.NetApiExt.Generated.Model.polkadot_runtime_common.slots.pallet
{
    
    
    public enum Call
    {
        
        force_lease = 0,
        
        clear_all_leases = 1,
        
        trigger_onboard = 2,
    }
    
    /// <summary>
    /// >> 412 - Variant[polkadot_runtime_common.slots.pallet.Call]
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class EnumCall : BaseEnumExt<Call, BaseTuple<PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Ajuna.NetApi.Model.Types.Primitive.U128, Ajuna.NetApi.Model.Types.Primitive.U32, Ajuna.NetApi.Model.Types.Primitive.U32>, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id>
    {
    }
}

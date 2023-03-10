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


namespace PlutoWallet.NetApiExt.Generated.Model.polkadot_runtime_common.paras_registrar.pallet
{
    
    
    public enum Call
    {
        
        register = 0,
        
        force_register = 1,
        
        deregister = 2,
        
        swap = 3,
        
        remove_lock = 4,
        
        reserve = 5,
        
        add_lock = 6,
        
        schedule_code_upgrade = 7,
        
        set_current_head = 8,
    }
    
    /// <summary>
    /// >> 411 - Variant[polkadot_runtime_common.paras_registrar.pallet.Call]
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class EnumCall : BaseEnumExt<Call, BaseTuple<PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.HeadData, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.ValidationCode>, BaseTuple<PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Ajuna.NetApi.Model.Types.Primitive.U128, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.HeadData, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.ValidationCode>, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, BaseTuple<PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id>, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, BaseVoid, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, BaseTuple<PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.ValidationCode>, BaseTuple<PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.Id, PlutoWallet.NetApiExt.Generated.Model.polkadot_parachain.primitives.HeadData>>
    {
    }
}
